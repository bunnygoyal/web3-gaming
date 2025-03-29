using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;

/// <summary>
/// Manages blockchain interactions using Claude MCP directly
/// </summary>
public class MCPBlockchainManager : MonoBehaviour
{
    // Reference to Unity MCP (will need to be set in Inspector)
    [SerializeField] private GameObject unityMCPManager;
    
    // UI references
    [SerializeField] private TextMeshProUGUI statusText;
    
    // Current connection state
    private string connectedAddress = "";
    private string currentChain = "Ethereum";
    
    // Cache for asset data
    private Dictionary<string, List<GameAsset>> assetCache = new Dictionary<string, List<GameAsset>>();
    
    // Supported chains
    private readonly string[] supportedChains = { "Ethereum", "Polygon", "Avalanche", "Binance Smart Chain" };
    
    void Start()
    {
        if (unityMCPManager == null)
        {
            Debug.LogError("Unity MCP Manager reference not set! Please assign in inspector.");
        }
        
        // Log that we're ready
        UpdateStatus("Ready to connect wallet...");
    }
    
    /// <summary>
    /// Connect to the player's wallet via MCP
    /// </summary>
    public async Task<string> ConnectWallet()
    {
        UpdateStatus("Connecting wallet...");
        
        try
        {
            // This is the prompt that will be sent to Claude via MCP
            string mcpPrompt = @"
            You are now interfacing with a blockchain wallet via Web3.
            
            1. Check if the browser has an Ethereum provider (MetaMask)
            2. Request connection to the user's wallet
            3. Return the connected address
            
            Use this exact JavaScript:
            ```javascript
            async function connectWallet() {
              if (window.ethereum) {
                try {
                  const accounts = await window.ethereum.request({ method: 'eth_requestAccounts' });
                  return accounts[0];
                } catch (error) {
                  console.error(error);
                  return 'ERROR: ' + error.message;
                }
              } else {
                return 'ERROR: MetaMask not installed';
              }
            }
            
            connectWallet();
            ```
            
            Return ONLY the wallet address or the error message.
            ";
            
            // In production, this would actually send the prompt to Claude via MCP
            // string response = await unityMCPManager.SendPromptToClaude(mcpPrompt);
            
            // For now, simulate a successful response
            await Task.Delay(1000); // Simulate network delay
            string response = "0x742d35Cc6634C0532925a3b844Bc454e4438f44e";
            
            // Check if the response is an error
            if (response.StartsWith("ERROR:"))
            {
                UpdateStatus("Connection failed: " + response.Substring(6));
                return response;
            }
            
            // Store the connected address
            connectedAddress = response;
            UpdateStatus("Connected: " + ShortenAddress(connectedAddress));
            
            return "ADDRESS:" + connectedAddress;
        }
        catch (Exception e)
        {
            string errorMsg = "Error connecting wallet: " + e.Message;
            UpdateStatus(errorMsg);
            return "ERROR:" + e.Message;
        }
    }
    
    /// <summary>
    /// Switch to a different blockchain via MCP
    /// </summary>
    public async Task<bool> SwitchChain(string chainName)
    {
        if (!Array.Exists(supportedChains, chain => chain == chainName))
        {
            UpdateStatus($"Unsupported chain: {chainName}");
            return false;
        }
        
        UpdateStatus($"Switching to {chainName}...");
        
        try
        {
            // This is the prompt that will be sent to Claude via MCP
            string mcpPrompt = GetChainSwitchPrompt(chainName);
            
            // In production, this would actually send the prompt to Claude via MCP
            // string response = await unityMCPManager.SendPromptToClaude(mcpPrompt);
            
            // For now, simulate a successful response
            await Task.Delay(1000); // Simulate network delay
            string response = "SUCCESS";
            
            // Check if the response is an error
            if (response.StartsWith("ERROR:"))
            {
                UpdateStatus("Chain switch failed: " + response.Substring(6));
                return false;
            }
            
            // Update the current chain
            currentChain = chainName;
            UpdateStatus($"Connected to {chainName}: {ShortenAddress(connectedAddress)}");
            
            return true;
        }
        catch (Exception e)
        {
            string errorMsg = "Error switching chain: " + e.Message;
            UpdateStatus(errorMsg);
            return false;
        }
    }
    
    /// <summary>
    /// Get all assets owned by the player on the current chain via MCP
    /// </summary>
    public async Task<List<GameAsset>> GetPlayerAssets()
    {
        if (string.IsNullOrEmpty(connectedAddress))
        {
            UpdateStatus("Not connected to wallet");
            return new List<GameAsset>();
        }
        
        // Check cache first
        string cacheKey = $"{currentChain}_{connectedAddress}";
        if (assetCache.ContainsKey(cacheKey))
        {
            return assetCache[cacheKey];
        }
        
        UpdateStatus($"Fetching assets on {currentChain}...");
        
        try
        {
            // This is the prompt that will be sent to Claude via MCP
            string mcpPrompt = $@"
            Query all NFTs owned by the wallet address {connectedAddress} on {currentChain}.
            
            For our sample game contract: 0xYourGameAssetContract
            
            Use the appropriate method to get all tokens owned by this address.
            For each asset, retrieve:
            - tokenId
            - assetType (call getAssetType(tokenId))
            - tokenURI
            - useCount (call getUseCount(tokenId))
            
            Format the response as a JSON array with these properties for each asset.
            ";
            
            // In production, this would actually send the prompt to Claude via MCP
            // string response = await unityMCPManager.SendPromptToClaude(mcpPrompt);
            
            // For now, simulate a successful response with sample data
            await Task.Delay(1500); // Simulate network delay
            string response = @"
            [
                {
                    ""tokenId"": ""1"",
                    ""assetType"": ""1"",
                    ""tokenURI"": ""https://example.com/assets/1"",
                    ""useCount"": ""0""
                },
                {
                    ""tokenId"": ""2"",
                    ""assetType"": ""2"",
                    ""tokenURI"": ""https://example.com/assets/2"",
                    ""useCount"": ""3""
                },
                {
                    ""tokenId"": ""5"",
                    ""assetType"": ""1"",
                    ""tokenURI"": ""https://example.com/assets/5"",
                    ""useCount"": ""1""
                }
            ]";
            
            // Parse the JSON response
            List<GameAsset> assets = JsonUtility.FromJson<List<GameAsset>>(response);
            
            // Cache the results
            assetCache[cacheKey] = assets;
            
            UpdateStatus($"Found {assets.Count} assets on {currentChain}");
            return assets;
        }
        catch (Exception e)
        {
            string errorMsg = "Error getting assets: " + e.Message;
            UpdateStatus(errorMsg);
            return new List<GameAsset>();
        }
    }
    
    /// <summary>
    /// Use an asset in the game via MCP
    /// </summary>
    public async Task<bool> UseAsset(string tokenId)
    {
        if (string.IsNullOrEmpty(connectedAddress))
        {
            UpdateStatus("Not connected to wallet");
            return false;
        }
        
        UpdateStatus($"Using asset {tokenId}...");
        
        try
        {
            // This is the prompt that will be sent to Claude via MCP
            string mcpPrompt = $@"
            Call the useAsset function for token ID {tokenId} on contract 0xYourGameAssetContract on {currentChain}.
            
            The useAsset function takes a single parameter: the tokenId.
            Make sure to use the connected address {connectedAddress} to sign the transaction.
            
            Return the transaction hash on success, or an error message on failure.
            ";
            
            // In production, this would actually send the prompt to Claude via MCP
            // string response = await unityMCPManager.SendPromptToClaude(mcpPrompt);
            
            // For now, simulate a successful response
            await Task.Delay(2000); // Simulate network delay
            string response = "0x8a7d953f45f84d5b339c5df82f6b42ce034793573d152f3e6791e32613906133";
            
            // Check if the response is an error
            if (response.StartsWith("ERROR:"))
            {
                UpdateStatus("Asset use failed: " + response.Substring(6));
                return false;
            }
            
            // Clear cache for this chain/address to force refresh
            string cacheKey = $"{currentChain}_{connectedAddress}";
            if (assetCache.ContainsKey(cacheKey))
            {
                assetCache.Remove(cacheKey);
            }
            
            UpdateStatus($"Asset {tokenId} used successfully!");
            return true;
        }
        catch (Exception e)
        {
            string errorMsg = "Error using asset: " + e.Message;
            UpdateStatus(errorMsg);
            return false;
        }
    }
    
    /// <summary>
    /// Get detailed metadata for an asset via MCP
    /// </summary>
    public async Task<AssetMetadata> GetAssetMetadata(string tokenURI)
    {
        UpdateStatus($"Fetching metadata...");
        
        try
        {
            // This is the prompt that will be sent to Claude via MCP
            string mcpPrompt = $@"
            Fetch the metadata from the tokenURI: {tokenURI}
            
            Parse the JSON response and extract:
            - name
            - description
            - image URL
            - attributes array
            
            Return the metadata as a JSON object with these properties.
            ";
            
            // In production, this would actually send the prompt to Claude via MCP
            // string response = await unityMCPManager.SendPromptToClaude(mcpPrompt);
            
            // For now, simulate a successful response
            await Task.Delay(1000); // Simulate network delay
            string response = @"
            {
                ""name"": ""Legendary Sword"",
                ""description"": ""A powerful weapon forged in dragon fire."",
                ""image"": ""https://example.com/images/sword.png"",
                ""attributes"": [
                    {
                        ""trait_type"": ""Damage"",
                        ""value"": 95
                    },
                    {
                        ""trait_type"": ""Element"",
                        ""value"": ""Fire""
                    },
                    {
                        ""trait_type"": ""Rarity"",
                        ""value"": ""Legendary""
                    }
                ]
            }";
            
            // Parse the JSON response
            AssetMetadata metadata = JsonUtility.FromJson<AssetMetadata>(response);
            
            UpdateStatus($"Loaded metadata for {metadata.name}");
            return metadata;
        }
        catch (Exception e)
        {
            string errorMsg = "Error getting metadata: " + e.Message;
            UpdateStatus(errorMsg);
            return new AssetMetadata
            {
                name = "Unknown Asset",
                description = "Could not load metadata",
                image = "https://example.com/images/placeholder.png"
            };
        }
    }
    
    // Helper methods
    
    /// <summary>
    /// Shorten an Ethereum address for display
    /// </summary>
    private string ShortenAddress(string address)
    {
        if (string.IsNullOrEmpty(address) || address.Length < 10)
            return address;
            
        return address.Substring(0, 6) + "..." + address.Substring(address.Length - 4);
    }
    
    /// <summary>
    /// Update the status text
    /// </summary>
    private void UpdateStatus(string message)
    {
        Debug.Log(message);
        
        if (statusText != null)
        {
            statusText.text = message;
        }
    }
    
    /// <summary>
    /// Get the chain switch prompt for a given chain
    /// </summary>
    private string GetChainSwitchPrompt(string chainName)
    {
        Dictionary<string, (string chainId, string rpcUrl)> chainConfigs = new Dictionary<string, (string, string)>
        {
            { "Ethereum", ("0x1", "https://mainnet.infura.io/v3/YOUR_API_KEY") },
            { "Polygon", ("0x89", "https://polygon-rpc.com") },
            { "Avalanche", ("0xa86a", "https://api.avax.network/ext/bc/C/rpc") },
            { "Binance Smart Chain", ("0x38", "https://bsc-dataseed.binance.org") }
        };
        
        (string chainId, string rpcUrl) = chainConfigs[chainName];
        
        return $@"
        You are now switching the connected wallet to {chainName} network.
        
        Use this exact JavaScript:
        ```javascript
        async function switchChain() {{
          if (window.ethereum) {{
            try {{
              await window.ethereum.request({{
                method: 'wallet_switchEthereumChain',
                params: [{{ chainId: '{chainId}' }}],
              }});
              return 'SUCCESS';
            }} catch (switchError) {{
              // This error code indicates the chain hasn't been added to MetaMask
              if (switchError.code === 4902) {{
                try {{
                  await window.ethereum.request({{
                    method: 'wallet_addEthereumChain',
                    params: [
                      {{
                        chainId: '{chainId}',
                        chainName: '{chainName}',
                        rpcUrls: ['{rpcUrl}'],
                      }},
                    ],
                  }});
                  return 'SUCCESS';
                }} catch (addError) {{
                  return 'ERROR: ' + addError.message;
                }}
              }}
              return 'ERROR: ' + switchError.message;
            }}
          }} else {{
            return 'ERROR: MetaMask not installed';
          }}
        }}
        
        switchChain();
        ```
        
        Return ONLY 'SUCCESS' or the error message.
        ";
    }
}

/// <summary>
/// Represents an in-game asset from the blockchain
/// </summary>
[Serializable]
public class GameAsset
{
    public string tokenId;
    public string assetType;
    public string tokenURI;
    public string useCount;
}

/// <summary>
/// Represents the metadata for an asset
/// </summary>
[Serializable]
public class AssetMetadata
{
    public string name;
    public string description;
    public string image;
    public List<AssetAttribute> attributes;
}

/// <summary>
/// Represents an attribute for an asset
/// </summary>
[Serializable]
public class AssetAttribute
{
    public string trait_type;
    public string value;
}
