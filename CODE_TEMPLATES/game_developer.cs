using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

// Game Developer: Copy these templates to kickstart your implementation

/// <summary>
/// Main game controller that coordinates between UI and blockchain systems
/// </summary>
public class GameController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject mainMenu;
    public GameObject inventoryPanel;
    public GameObject loadingPanel;
    public Text walletAddressText;
    public Dropdown chainSelector;
    public Transform assetContainer;
    public GameObject assetPrefab;
    
    [Header("References")]
    public WalletConnector walletConnector; // This will be implemented by Blockchain Specialist
    
    // Called by UI button
    public async void ConnectWalletButton()
    {
        loadingPanel.SetActive(true);
        
        // This will be implemented by Blockchain Specialist
        bool success = await walletConnector.ConnectWallet();
        
        if (success)
        {
            string address = PlayerPrefs.GetString("WalletAddress");
            walletAddressText.text = GetShortenedAddress(address);
            mainMenu.SetActive(false);
            inventoryPanel.SetActive(true);
            LoadPlayerAssets();
        }
        
        loadingPanel.SetActive(false);
    }
    
    // Called when chain selection changes
    public void OnChainSelected(int index)
    {
        // These will be implemented by Blockchain Specialist
        string[] chains = { "Ethereum", "Polygon", "Avalanche", "Binance" };
        string selectedChain = chains[index];
        
        // Call blockchain specialist's method
        // Example: blockchainManager.SwitchChain(selectedChain);
        
        // Reload assets for the new chain
        LoadPlayerAssets();
    }
    
    // Loads player's assets - will be connected to blockchain specialist's code
    private async void LoadPlayerAssets()
    {
        loadingPanel.SetActive(true);
        
        // Clear existing assets
        foreach (Transform child in assetContainer)
        {
            Destroy(child.gameObject);
        }
        
        // This will be where blockchain specialist's code is called
        // Example: 
        // var assets = await blockchainManager.GetPlayerAssets();
        // foreach (var asset in assets) { CreateAssetUI(asset); }
        
        // For now, create placeholder assets
        for (int i = 0; i < 5; i++)
        {
            CreatePlaceholderAsset($"Asset #{i}");
        }
        
        loadingPanel.SetActive(false);
    }
    
    // Creates UI for a placeholder asset
    private void CreatePlaceholderAsset(string name)
    {
        GameObject assetObj = Instantiate(assetPrefab, assetContainer);
        AssetDisplay display = assetObj.GetComponent<AssetDisplay>();
        
        if (display != null)
        {
            display.SetPlaceholderData(name, "This is a placeholder asset");
        }
    }
    
    // Utility to shorten wallet addresses for display
    private string GetShortenedAddress(string address)
    {
        if (string.IsNullOrEmpty(address) || address.Length < 10)
            return address;
            
        return address.Substring(0, 6) + "..." + address.Substring(address.Length - 4);
    }
}

/// <summary>
/// Displays a single asset/NFT in the UI
/// </summary>
public class AssetDisplay : MonoBehaviour
{
    public RawImage assetImage;
    public Text assetNameText;
    public Text assetDescriptionText;
    public Button useButton;
    
    private string tokenId;
    
    // Set up with placeholder data during development
    public void SetPlaceholderData(string name, string description)
    {
        assetNameText.text = name;
        assetDescriptionText.text = description;
        
        // Create a colored texture as placeholder
        Color color = new Color(
            Random.Range(0.5f, 1f),
            Random.Range(0.5f, 1f),
            Random.Range(0.5f, 1f)
        );
        
        Texture2D texture = new Texture2D(64, 64);
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        
        assetImage.texture = texture;
    }
    
    // Will be used later to display real asset data
    public async Task DisplayAsset(string id, string uri)
    {
        tokenId = id;
        
        // This will be implemented to fetch real NFT metadata
        // Example: var metadata = await blockchainManager.GetTokenMetadata(uri);
        
        // For now, use placeholder data
        SetPlaceholderData($"Asset #{id}", "Real asset data will be loaded here");
    }
    
    // Called when player wants to use this asset in game
    public void OnUseButtonClick()
    {
        // This will be where game mechanics integrate with the blockchain asset
        Debug.Log($"Using asset {tokenId} in game");
        
        // Example: GameMechanics.ApplyItemEffect(tokenId);
    }
}
