# Blockchain Specialist Tasks (2-3 Day Sprint)

## Your Role
As the Blockchain Specialist, you'll handle all on-chain interactions. The Game Developer will build the Unity UI and gameplay mechanics - you'll provide the blockchain connection layer that powers their interface.

## Day 1: Setup & Foundation

### Morning (3 hours)
- [ ] **Set up development environment**
  - Install Git, Node.js, and necessary blockchain tools
  - Create accounts on test networks (Mumbai, Goerli, Fuji)
  - Get test ETH for each network
  
- [ ] **Configure Unity MCP for AI assistance**
  ```bash
  git clone https://github.com/justinpbarnett/unity-mcp.git
  ```
  - Install UV package manager:
    ```bash
    # Windows
    powershell -c "irm https://astral.sh/uv/install.ps1 | iex"
    # Mac
    brew install uv
    ```
  - Install dependencies: `uv pip install -e .`
  
- [ ] **Choose and set up blockchain SDK**
  - Option 1: Web3.unity by ChainSafe
    ```
    - Download from Unity Asset Store
    - Configure for multi-chain support
    ```
    
  - Option 2: ThirdWeb SDK
    ```
    - Import from Unity Package Manager
    - Add ThirdWebManager prefab to scene
    ```
    
  - Option 3: Moralis Unity SDK
    ```
    - Import from GitHub or Asset Store
    - Configure server connection
    ```

### Afternoon (5 hours)
- [ ] **Implement wallet connection module**
  ```csharp
  public class WalletConnector : MonoBehaviour
  {
      public async Task<bool> ConnectWallet()
      {
          // SDK-specific implementation here
          // e.g. for ThirdWeb:
          // var sdk = new ThirdwebSDK("ethereum");
          // string address = await sdk.wallet.Connect();
      }
  }
  ```
  
- [ ] **Create contract interaction service**
  - Define interface for game assets
  - Set up functions for retrieving player assets
  - Create methods for minting/transferring items
  
- [ ] **Set up multi-chain configuration**
  ```csharp
  public class ChainConfig
  {
      public string chainId;
      public string network;
      public string contractAddress;
  }
  
  // Create configurations for each chain
  Dictionary<string, ChainConfig> chainConfigs = new Dictionary<string, ChainConfig>()
  {
      { "Ethereum", new ChainConfig("ethereum", "mainnet", "0x...") },
      { "Polygon", new ChainConfig("polygon", "mainnet", "0x...") },
      { "Avalanche", new ChainConfig("avalanche", "mainnet", "0x...") }
  };
  ```
  
- [ ] **Deploy basic test contracts**
  - Create simple ERC-721 contract for game assets
  - Deploy to Polygon Mumbai testnet
  - Verify contract on block explorer

## Day 2: Core Implementation

### Morning (4 hours)
- [ ] **Implement asset minting**
  ```csharp
  public async Task<string> MintGameAsset(string tokenURI)
  {
      // SDK-specific implementation
      // e.g. for Web3.unity:
      // var contract = new Contract(abi, contractAddress);
      // var tx = await contract.Send("mintToken", tokenURI);
      // return tx.transactionHash;
  }
  ```
  
- [ ] **Create chain-switching logic**
  ```csharp
  public async Task<bool> SwitchChain(string chainName)
  {
      if (chainConfigs.TryGetValue(chainName, out ChainConfig config))
      {
          // SDK-specific implementation
          // e.g. for ThirdWeb:
          // sdk = new ThirdwebSDK(config.chainId);
          return true;
      }
      return false;
  }
  ```
  
- [ ] **Build player inventory system**
  - Create functions to fetch owned NFTs
  - Set up metadata retrieval
  - Implement asset ownership verification

### Afternoon (4 hours)
- [ ] **Implement transaction signing**
  - Create robust error handling
  - Add gas estimation
  - Set up transaction monitoring
  
- [ ] **Create caching system**
  - Cache NFT metadata locally
  - Store transaction history
  - Implement asset thumbnail caching
  
- [ ] **Connect blockchain functions to UI hooks**
  - Create public methods for Game Developer to call
  - Implement event system for blockchain updates
  - Add callbacks for transaction states

## Day 3: Testing & Deployment

### Morning (4 hours)
- [ ] **Deploy final contracts**
  - Finalize smart contract code
  - Deploy to mainnet (if budget allows) or retain on testnets
  - Set up contract verification
  
- [ ] **Optimize gas usage**
  - Batch transactions where possible
  - Implement gas estimation
  - Add circuit breakers for high gas situations
  
- [ ] **Set up contract monitoring**
  - Implement event listeners
  - Create transaction tracking
  - Add logging for contract interactions

### Afternoon (4 hours)
- [ ] **Conduct security audit**
  - Review all contract interactions
  - Check for common vulnerabilities
  - Implement safeguards
  
- [ ] **Test mainnet connections**
  - Verify all chains work correctly
  - Test asset retrieval across chains
  - Verify transaction signing
  
- [ ] **Create SDK documentation**
  - Document all public methods
  - Create examples for Game Developer
  - Add error code explanations

## SDK Options Comparison

| SDK | Pros | Cons | Best For |
|-----|------|------|----------|
| **Web3.unity (ChainSafe)** | - Blockchain-agnostic<br>- Supports Ethereum, Avalanche, BSC, Moonbeam, Polygon, xDai<br>- Solid documentation | - More complex API<br>- Some features require more code | Teams with blockchain experience |
| **ThirdWeb Unity SDK** | - Easy to use<br>- Pre-built prefabs<br>- Good for NFTs and marketplaces | - Less flexible<br>- Tied to ThirdWeb ecosystem | Fast implementation with less code |
| **Moralis Unity SDK** | - Powerful backend<br>- Good for complex dApps<br>- Extensive features | - Requires Moralis server<br>- Learning curve | Apps needing backend services |

## Optional: Bankless Onchain MCP
Consider setting up [Bankless Onchain MCP](https://github.com/Bankless/onchain-mcp) to let Claude access blockchain data directly. This is useful for debugging and can help you develop faster with AI assistance for blockchain data:

```bash
npm install -g @bankless/onchain-mcp
npx @bankless/onchain-mcp
```

## Code Templates

Use the `CODE_TEMPLATES/blockchain_specialist.cs` file for starter code. The key components you need to implement are:

1. **WalletConnector**: Handles wallet connection and authentication
2. **BlockchainManager**: Manages multi-chain support and contract interactions 
3. **AssetController**: Handles NFT-related functionality

## Integration Notes

- Your code will be called by the Game Developer's UI logic
- Focus on making your API as simple as possible for them to use
- Provide clear error messages and status updates
- Meet with the Game Developer at the end of each day to ensure proper integration

## Resources

- **Smart Contract Templates**: [OpenZeppelin Contracts](https://github.com/OpenZeppelin/openzeppelin-contracts)
- **Gas Optimization Guides**: [Ethereum Gas Optimization](https://ethereum.org/en/developers/docs/gas-optimization/)
- **Web3.unity Documentation**: [ChainSafe Docs](https://docs.gaming.chainsafe.io/)
- **ThirdWeb Documentation**: [ThirdWeb Unity Portal](https://portal.thirdweb.com/unity)
