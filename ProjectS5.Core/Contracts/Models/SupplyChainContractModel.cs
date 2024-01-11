using System.ComponentModel.DataAnnotations;

namespace ProjectS5.Core.Contracts.Models;

public class SupplyChainContractModel
{
    public const string Code = "pragma solidity ^0.8.0;\r\n\r\ncontract SupplyChainContract {\r\n    // User-specific data fields\r\n    string public itemID;\r\n    string public itemName;\r\n    string public origin;\r\n    address public owner;             \r\n\r\n    enum Stages {\r\n        Created,\r\n        Shipped,\r\n        Delivered\r\n    }\r\n\r\n    Stages public currentStage;\r\n\r\n    constructor(string memory _itemID, string memory _itemName, string memory _origin, address _owner) {\r\n        itemID = _itemID;\r\n        itemName = _itemName;\r\n        origin = _origin;\r\n        owner = _owner;\r\n        currentStage = Stages.Created;\r\n    }\r\n\r\n    function updateStage(Stages _stage) public {\r\n        require(msg.sender == owner, \"Only the owner can update the stage\");\r\n        currentStage = _stage;\r\n    }\r\n\r\n    function getItemDetails() public view returns (string memory, string memory, string memory, Stages) {\r\n        return (itemID, itemName, origin, currentStage);\r\n    }\r\n}";

    [Required]
    public string ItemId { get; set; }

    [Required]
    public string ItemName { get; set; }

    [Required]
    public string Origin { get; set; }

    [Required]
    public string EthAddress { get; set; }
}
