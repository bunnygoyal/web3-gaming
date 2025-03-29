// SPDX-License-Identifier: MIT
pragma solidity ^0.8.9;

import "@openzeppelin/contracts/token/ERC721/ERC721.sol";
import "@openzeppelin/contracts/token/ERC721/extensions/ERC721Enumerable.sol";
import "@openzeppelin/contracts/token/ERC721/extensions/ERC721URIStorage.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
import "@openzeppelin/contracts/utils/Counters.sol";

/**
 * @title GameAsset
 * @dev ERC721 contract for in-game assets with multi-chain support
 */
contract GameAsset is ERC721, ERC721Enumerable, ERC721URIStorage, Ownable {
    using Counters for Counters.Counter;

    Counters.Counter private _tokenIdCounter;
    
    // Mapping from token ID to asset type
    mapping(uint256 => uint8) private _assetTypes;
    
    // Events
    event AssetMinted(address indexed player, uint256 tokenId, uint8 assetType);
    event AssetUsed(address indexed player, uint256 tokenId, uint32 useCount);
    
    // Asset usage tracking
    mapping(uint256 => uint32) private _useCount;
    
    constructor() ERC721("GameAsset", "GAME") {}

    /**
     * @dev Mint a new game asset
     * @param to The address that will own the minted asset
     * @param assetType The type of asset (1=weapon, 2=armor, 3=potion, etc.)
     * @param uri The metadata URI for the asset
     */
    function mintAsset(address to, uint8 assetType, string memory uri) public onlyOwner {
        uint256 tokenId = _tokenIdCounter.current();
        _tokenIdCounter.increment();
        
        _safeMint(to, tokenId);
        _setTokenURI(tokenId, uri);
        _assetTypes[tokenId] = assetType;
        
        emit AssetMinted(to, tokenId, assetType);
    }
    
    /**
     * @dev Allow players to use their asset in-game
     * @param tokenId The ID of the asset to use
     */
    function useAsset(uint256 tokenId) public {
        require(_isApprovedOrOwner(_msgSender(), tokenId), "Not approved to use this asset");
        
        // Increment use count
        _useCount[tokenId]++;
        
        emit AssetUsed(_msgSender(), tokenId, _useCount[tokenId]);
    }
    
    /**
     * @dev Get the type of an asset
     * @param tokenId The ID of the asset
     * @return The asset type
     */
    function getAssetType(uint256 tokenId) public view returns (uint8) {
        require(_exists(tokenId), "Asset does not exist");
        return _assetTypes[tokenId];
    }
    
    /**
     * @dev Get the number of times an asset has been used
     * @param tokenId The ID of the asset
     * @return The use count
     */
    function getUseCount(uint256 tokenId) public view returns (uint32) {
        require(_exists(tokenId), "Asset does not exist");
        return _useCount[tokenId];
    }
    
    /**
     * @dev Get all tokens owned by an address
     * @param owner The address to check
     * @return An array of token IDs owned by the address
     */
    function tokensOfOwner(address owner) public view returns (uint256[] memory) {
        uint256 ownerTokenCount = balanceOf(owner);
        uint256[] memory tokenIds = new uint256[](ownerTokenCount);
        
        for (uint256 i = 0; i < ownerTokenCount; i++) {
            tokenIds[i] = tokenOfOwnerByIndex(owner, i);
        }
        
        return tokenIds;
    }
    
    // The following functions are overrides required by Solidity

    function _beforeTokenTransfer(address from, address to, uint256 tokenId, uint256 batchSize)
        internal
        override(ERC721, ERC721Enumerable)
    {
        super._beforeTokenTransfer(from, to, tokenId, batchSize);
    }

    function _burn(uint256 tokenId) internal override(ERC721, ERC721URIStorage) {
        super._burn(tokenId);
    }

    function tokenURI(uint256 tokenId)
        public
        view
        override(ERC721, ERC721URIStorage)
        returns (string memory)
    {
        return super.tokenURI(tokenId);
    }

    function supportsInterface(bytes4 interfaceId)
        public
        view
        override(ERC721, ERC721Enumerable)
        returns (bool)
    {
        return super.supportsInterface(interfaceId);
    }
}
