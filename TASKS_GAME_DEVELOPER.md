# Game Developer Tasks (2-3 Day Sprint)

## Your Role
As the Game Developer, you'll focus exclusively on Unity gameplay and UI elements. The Blockchain Specialist will handle all chain interactions - you don't need to understand the blockchain details, just integrate with their code.

## Day 1: Setup & Foundation

### Morning (3 hours)
- [ ] **Install Unity 2020.3 LTS or newer**
  - Note: Make sure it's URP compatible
  - Verify your hardware meets requirements
  
- [ ] **Setup Unity MCP for AI assistance**
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
  - In Unity: Window > Unity MCP > Configurator > Auto Configure
  
- [ ] **Create basic game scene structure**
  - Main menu scene
  - Gameplay scene 
  - Inventory display scene
  - Settings scene

### Afternoon (5 hours)
- [ ] **Design UI for wallet connection** 
  - Create simple connect button
  - Add wallet address display field
  - Create loading indicator
  
- [ ] **Build asset display framework**
  - Create asset card prefab
  - Design scrollable asset grid
  - Add placeholder for asset images
  
- [ ] **Create chain selection UI**
  - Add dropdown menu with main chains:
    - Ethereum
    - Polygon
    - Avalanche
    - Binance Smart Chain
  
- [ ] **Implement UI event hooks**
  ```csharp
  // Example of wallet connect button event
  public void OnConnectWalletClicked()
  {
      // This will be connected to blockchain code later
      // Show loading UI
      loadingPanel.SetActive(true);
      // Call will be added: walletConnector.ConnectWallet();
  }
  ```

## Day 2: Core Implementation

### Morning (4 hours)
- [ ] **Finalize UI elements**
  - Polish all UI screens
  - Add animations and transitions
  - Ensure responsive layout works on different resolutions
  
- [ ] **Implement asset visualization**
  - Create thumbnail loading system
  - Design asset detail view
  - Add placeholder visualization for NFTs
  
- [ ] **Create transaction feedback UI**
  - Add progress indicators
  - Create success/error messaging
  - Design confirmation dialogs

### Afternoon (4 hours)
- [ ] **Implement basic gameplay mechanics**
  - Design how NFTs will function in-game
  - Create simple interaction system
  - Add effects for asset usage
  
- [ ] **Create onboarding tutorial**
  - First-time user experience
  - Wallet connection guide
  - Basic gameplay instructions
  
- [ ] **Polish user experience**
  - Add sound effects
  - Improve visual feedback
  - Optimize loading times

## Day 3: Finalization & Testing

### Morning (4 hours)
- [ ] **Conduct user testing**
  - Test all UI flows
  - Verify asset display works correctly
  - Check responsiveness across different screen sizes
  
- [ ] **Implement feedback from testing**
  - Fix any UI issues
  - Address gameplay concerns
  - Improve user experience pain points
  
- [ ] **Add final polish**
  - Refine animations
  - Add particle effects
  - Improve color scheme and visual hierarchy

### Afternoon (4 hours)
- [ ] **Optimize performance**
  - Remove unnecessary GameObjects
  - Batch similar materials
  - Reduce draw calls
  
- [ ] **Prepare build settings**
  - Configure for target platforms
  - Set up correct screen resolution options
  - Verify all scenes are included
  
- [ ] **Create final build**
  - Generate release build
  - Test on target platform
  - Verify all features work in build

## Code Templates

Use the `CODE_TEMPLATES/game_developer.cs` file for starter code. The key components you need to implement are:

1. **GameController**: Main orchestrator that handles UI state and communicates with blockchain code
2. **AssetDisplay**: Displays individual NFTs in the game UI
3. **Chain selection logic**: UI for switching between blockchains

## Integration Notes

- Your code will interface with the Blockchain Specialist's code through simple function calls
- You don't need to understand the blockchain details - just call their functions when appropriate
- The interface will be primarily through the `WalletConnector` and `BlockchainManager` classes
- Meet with the Blockchain Specialist at the end of each day to connect your systems

## Resources

- **UI Kit Resources**: Use pre-made UI kits from the Unity Asset Store to speed up development
- **Unity MCP Documentation**: [GitHub Repository](https://github.com/justinpbarnett/unity-mcp)
- **Stock Art**: Use placeholder art from sites like [OpenGameArt](https://opengameart.org/)
