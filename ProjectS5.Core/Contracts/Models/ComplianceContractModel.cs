using System.ComponentModel.DataAnnotations;

namespace ProjectS5.Core.Contracts.Models;

public class ComplianceContractModel
{
    public const string Code = "pragma solidity ^0.8.0;\r\n\r\ncontract CustomsComplianceContract {\r\n    struct Item {\r\n        string description;\r\n        bool isCompliant;\r\n        uint256 customsDuty;\r\n        bool dutyPaid;\r\n    }\r\n\r\n    address public owner;\r\n    mapping(string => Item) public items;\r\n\r\n    event ItemRegistered(string itemId, string description, bool isCompliant, uint256 customsDuty);\r\n    event DutyPaid(string itemId, uint256 amount);\r\n\r\n    constructor() {\r\n        owner = msg.sender;\r\n    }\r\n\r\n    function registerItem(string memory itemId, string memory description, bool isCompliant, uint256 customsDuty) public {\r\n        require(msg.sender == owner, \"Only the owner can register items.\");\r\n        require(items[itemId].customsDuty == 0, \"Item already registered.\");\r\n\r\n        items[itemId] = Item({\r\n            description: description,\r\n            isCompliant: isCompliant,\r\n            customsDuty: customsDuty,\r\n            dutyPaid: false\r\n        });\r\n\r\n        emit ItemRegistered(itemId, description, isCompliant, customsDuty);\r\n    }\r\n\r\n    function payDuty(string memory itemId) public payable {\r\n        Item storage item = items[itemId];\r\n\r\n        require(item.customsDuty > 0, \"Item not registered.\");\r\n        require(msg.value == item.customsDuty, \"Incorrect duty amount\");\r\n        require(!item.dutyPaid, \"Duty already paid\");\r\n        require(item.isCompliant, \"Item is not compliant\");\r\n\r\n        item.dutyPaid = true;\r\n        emit DutyPaid(itemId, msg.value);\r\n    }\r\n\r\n    function isDutyPaid(string memory itemId) public view returns (bool) {\r\n        return items[itemId].dutyPaid;\r\n    }\r\n}";

    [Required]
    public string ItemId { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public bool IsCompliant { get; set; }

    [Required, Range(0, double.MaxValue)]
    public decimal? CustomsDuty { get; set; }
}