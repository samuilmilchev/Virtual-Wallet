// This is our index.js file

/*
Create an instance of the secure fields. Pass the public key as the first argument. 
As the second argument, we'll pass a list of custom fonts to be applied to the fields (this is optional).
*/
const fonts = [
    {
        src: 'https://fonts.googleapis.com/css?family=Source+Code+Pro',
    }
]
const formElements = new POS.Fields("99346d84-5187-4221-9c16-dc51a8de27fa", {
    fonts
})


/*
Create an object holding additional options that you can pass to the constructor for instantiating 
the credit card and card expiry fields.
There are lots of other options available that you can pass to the constructor, 
but to keep it simple we'll just show this one object in our example. 
*/
const placeholders = {
    cardNumber: '1234 1234 1234 1234',
    expDate: 'MM / YY'
}

// Instantiate the fields you want to show and mount them to the DOM.
const cardNumber = formElements.create('cardNumber', {
    placeholders
})
cardNumber.mount('#card-number')

const expiry = formElements.create('creditCardExpiry', {
    placeholders
})
expiry.mount('#exp-date')

/*
Create a token when the user submits the form, but not until we fetched the card holder's name 
so that we can pass it in an additional data object to the createToken call.
*/
document.getElementById('payment-form').addEventListener('submit', async (event) => {
    event.preventDefault()
    const additionalData = {
        holder_name: document.getElementById('cardholder-name').value // This field is mandatory
    }
    const result = await POS.createToken(cardNumber, {
        additionalData,
        environment: 'test' // Set the PaymentsOS environment you're connecting to
    })
    console.log(`The response is ${JSON.stringify(result)}`)
})