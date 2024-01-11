function isMetaMaskInstalled() {
    return typeof window.ethereum !== 'undefined' && window.ethereum.isMetaMask === true;
}

async function connectMetaMask() {
    if (!isMetaMaskInstalled())
    {
        alert("MetaMask is not installed. Please install it to use this feature.");
        return null;
    }

    // Check if already connected
    const accounts = await window.ethereum.request({ method: 'eth_accounts' });
    if (accounts.length > 0) {
        return accounts[0]; // Already connected, return the first account
    }

    try {
        const newAccounts = await window.ethereum.request({ method: 'eth_requestAccounts' });
        return newAccounts[0];
    } catch (error) {
        console.error(error);
        return null;
    }
}

async function deployContract(abi, bytecode, userAddress) {
    // Create a web3 instance using MetaMask's provider
    const web3 = new Web3(window.ethereum);

    const parsedAbi = JSON.parse(abi);

    // Create contract instance
    const contract = new web3.eth.Contract(parsedAbi);

    // Estimate Gas
    const gasEstimate = await contract.deploy({
        data: bytecode
    }).estimateGas();

    return contract.deploy({
        data: bytecode
    }).send({
        from: userAddress,
        gas: gasEstimate
    });
}

