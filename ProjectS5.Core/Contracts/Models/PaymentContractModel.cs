using System.ComponentModel.DataAnnotations;

namespace ProjectS5.Core.Contracts.Models;

public class PaymentContractModel
{
    public const string Code = "pragma solidity ^0.8.0;\r\n\r\ncontract PaymentAndInvoiceContract {\r\n    // User-specific data fields\r\n    address public clientAddress;\r\n    string public invoiceId;\r\n    uint256 public amountDue;\r\n    string public description;\r\n\r\n    bool public isPaid;\r\n    address private owner;\r\n\r\n    event PaymentReceived(string invoiceId, uint256 amount);\r\n\r\n    constructor(address _clientAddress, string memory _invoiceId, uint256 _amountDue, string memory _description) {\r\n        clientAddress = _clientAddress;\r\n        invoiceId = _invoiceId;\r\n        amountDue = _amountDue;\r\n        description = _description;\r\n        owner = msg.sender;\r\n        isPaid = false;\r\n    }\r\n\r\n    function payInvoice() external payable {\r\n        require(msg.sender == clientAddress, \"Only the client can pay this invoice\");\r\n        require(msg.value == amountDue, \"Incorrect payment amount\");\r\n        require(!isPaid, \"This invoice has already been paid\");\r\n\r\n        (bool sent, ) = owner.call{value: msg.value}(\"\");\r\n        require(sent, \"Failed to send Ether\");\r\n        isPaid = true;\r\n\r\n        emit PaymentReceived(invoiceId, msg.value);\r\n    }\r\n\r\n    function getInvoiceDetails() external view returns (address, string memory, uint256, string memory, bool) {\r\n        return (clientAddress, invoiceId, amountDue, description, isPaid);\r\n    }\r\n}";
    
    [Required]
    public string EthAddress { get; set; }

    [Required]
    public string InvoiceId { get; set; }

    [Required, Range(0, double.MaxValue)]
    public decimal? Amount { get; set; }

    [Required]
    public string InvoiceDescription { get; set; }
}
