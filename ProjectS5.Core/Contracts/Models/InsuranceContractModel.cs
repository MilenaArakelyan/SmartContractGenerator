using System.ComponentModel.DataAnnotations;

namespace ProjectS5.Core.Contracts.Models;

public class InsuranceContractModel
{
    public const string Code = "pragma solidity ^0.8.0;\r\n\r\ncontract InsuranceContract {\r\n    struct Policy {\r\n        string description;\r\n        uint256 premium;\r\n        uint256 coverage;\r\n        bool active;\r\n        bool claimed;\r\n    }\r\n\r\n    address public owner;\r\n    mapping(string => Policy) public policies;\r\n\r\n    event PolicyCreated(string policyId, string description, uint256 premium, uint256 coverage);\r\n    event PremiumPaid(string policyId, uint256 amount);\r\n    event ClaimFiled(string policyId, uint256 amount);\r\n\r\n    constructor() {\r\n        owner = msg.sender;\r\n    }\r\n\r\n    function createPolicy(string memory policyId, string memory description, uint256 premium, uint256 coverage) public {\r\n        require(msg.sender == owner, \"Only the owner can create policies.\");\r\n        require(policies[policyId].coverage == 0, \"Policy already exists.\");\r\n\r\n        policies[policyId] = Policy({\r\n            description: description,\r\n            premium: premium,\r\n            coverage: coverage,\r\n            active: false,\r\n            claimed: false\r\n        });\r\n\r\n        emit PolicyCreated(policyId, description, premium, coverage);\r\n    }\r\n\r\n    function payPremium(string memory policyId) public payable {\r\n        Policy storage policy = policies[policyId];\r\n\r\n        require(policy.coverage > 0, \"Policy does not exist.\");\r\n        require(msg.value == policy.premium, \"Incorrect premium amount\");\r\n        require(!policy.active, \"Premium already paid\");\r\n\r\n        policy.active = true;\r\n        emit PremiumPaid(policyId, msg.value);\r\n    }\r\n\r\n    function fileClaim(string memory policyId) public {\r\n        Policy storage policy = policies[policyId];\r\n\r\n        require(policy.active, \"Policy is not active.\");\r\n        require(!policy.claimed, \"Claim already filed\");\r\n\r\n        policy.claimed = true;\r\n        // Logic to handle the claim (e.g., transferring coverage amount)\r\n        // For simplicity, we're just marking the claim as filed\r\n        emit ClaimFiled(policyId, policy.coverage);\r\n    }\r\n}";

    [Required]
    public string PolicyId { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public decimal? Premium { get; set; }

    [Required]
    public decimal? Coverage { get; set; }
}