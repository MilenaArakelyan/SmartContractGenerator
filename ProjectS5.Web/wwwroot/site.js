function copyToClipboard() {
    var code = document.querySelector('.code-container code').innerText;
    var tempElement = document.createElement('textarea');
    tempElement.value = code;
    document.body.appendChild(tempElement);
    tempElement.select();
    document.execCommand('copy');
    document.body.removeChild(tempElement);
    alert('Code copied to clipboard!');
}