function submitTransfer() {
    var transferType = document.getElementById("transferType").value;
    var amount = document.getElementById("amount").value;

    var transferRequest = {
        TransferType = transferType
        Amount: parseFloat(amount)

    };

    fetch('/api/transfer/transferFunds?transferType=' + encodeURIComponent(transferType), {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(transferRequest),
    })
        .then(async response => {
            let data;
            try {
                // Attempt to parse the response as JSON
                data = await response.json();
            } catch (error) {
                // If JSON parsing fails, fall back to plain text
                const text = await response.text();
                throw new Error(text || 'Unknown error occurred');
            }

            if (!response.ok) {
                // If the response was not successful, throw an error with the message from the response
                throw new Error(data.message || 'Transfer failed.');
            }

            // If the request was successful, display the success message
            document.getElementById("result").innerHTML = "Transfer successful!";
        })
        .catch(error => {
            // Handle errors (whether they are from failed fetch or server response)
            console.error('Error:', error);
            document.getElementById("result").innerHTML = `Error occurred during transfer: ${error.message}`;
        });
}
