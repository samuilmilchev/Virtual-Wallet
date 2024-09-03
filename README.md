# Nimble Wallet

## Description
Nimble Wallet is a robust web application built with ASP.NET Core that allows users to manage their budget conveniently. Users can send and receive money, add funds to their virtual wallet from a bank card, and track their transaction history with ease. The application provides essential features to ensure seamless and secure transactions, along with several optional features to enhance the user experience.

## Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/samuilmilchev/Virtual-Wallet.git
    cd [your-local-repo]
    ```
    example: cd E:\Telerik Academy\FINAL\Virtual-Wallet\.git
   
 2.Install-Packages   
 The needed packages will be added once you clone the repository.

3. Update the database:
    ```in Packet Manager Console
    
    Update-Database
    
    ```
4. Run the application:
    ```bash
    dotnet run
    ```

## Usage
Home page(without login):
![image](https://github.com/user-attachments/assets/adc21053-cf25-45f8-b35d-e9fd302daf63)
This is the homepage of Nimble virtual wallet service, emphasizing its features, team, and customer satisfaction, while also providing essential company information and user-friendly navigation. If a user is not logged or registrated, he has acces to "Home", "About" , "Login". There is also a so-called "burger" menu on the top right of the page, that is only accessible for authenticated users.

Registration form:
![image](https://github.com/user-attachments/assets/e7a58b39-1d20-450d-a62e-cb0d9d20806f)

By clicking the "Login" button you will be redirected to the Sign in/Sign up form. In order to register, you should click on Sign-up as shown and type in username , e-mail, password , phone number , avatar image(optional) , and your virual wallet's name.

After clicking the "Sign-up" button (and if all field are valid) you will be redirected to a success page that describes the additional steps to set-up your profile.
![image](https://github.com/user-attachments/assets/91dd9854-9fc1-4a8a-9a3c-14db0602b2b1)

At the same time you will get a confirmation e-mail to the e-mail address given from the registration form: 
![image](https://github.com/user-attachments/assets/902d879a-49a3-4427-8baf-bca98d20d171)

By clicking the highlighted text "Confrim Email" you will authenticate your profile and will be redirected to a email confirmation success page:
![image](https://github.com/user-attachments/assets/c7cfb4a1-9e54-4ae6-906c-c62765e88452)

After all the steps you can easily access the Sign-in form(in the login page) and log in : 
![image](https://github.com/user-attachments/assets/023c107b-f220-42b5-a5c4-8ef3e4d2c1b3)

Now you have access to the "burger" menu: 
![image](https://github.com/user-attachments/assets/e42872ca-0a29-4a8c-b6f7-4e62f169685c)

User Details page, with the user's profile details, where he can edit and/or verify his profile:
![image](https://github.com/user-attachments/assets/5c4b16b9-de07-4f2e-a8fa-15825ddda09f)

In order to be able to send money to other users, we should be verified by an admin, which is a additional step for security measures.

That being said, by clicking on "Verify" in the user details page you will be redirected to a page where you should upload a selfie and a photo of your ID, so the admin can verify your profile:
![image](https://github.com/user-attachments/assets/50be40e6-c71d-4b8f-a411-bb04ec57f476)

After a successfull verification by the admin, the user receives an e-mail as a notification for the verification:
![image](https://github.com/user-attachments/assets/0a84b7d6-447f-4d4d-8d58-53d9a47d7050)


From that moment on, the user can send money to other users' virtual wallets (if in advance the user has registered a card and transfered funds to his virtual wallet) by clicking on the "Send Money" button from the burger menu:
![image](https://github.com/user-attachments/assets/3c4c98db-fc16-4331-b069-24398eb2577b)

A user must reister a debit/credit card in order to add funds to his wallet.
Register Card page : 
![image](https://github.com/user-attachments/assets/f5f3a0d7-59ad-496d-beac-f934cfd35fe0)


You can view and manage your registered cards by accessing the "My Cards" page:
![image](https://github.com/user-attachments/assets/378d0d27-5b39-4128-89ad-8c42ea318a51)


After a successfull card register you will get a success message. You can then proceed to transfer money from your card to your virtual wallet.
This is possible in the Transfer Funds page:
![image](https://github.com/user-attachments/assets/67009895-6dba-452e-84a2-2971bc5a0428)
You can select from the drop-down menu to transfer funds from card to wallet("toWallet" , The money will be transfered to your main BGN wallet) or from wallet to card("toCard" , extract money from virtual wallet).

You can view your wallets(each one for it's respectful currency) in  the "My Wallets" page :
![image](https://github.com/user-attachments/assets/a400af75-3708-43f3-8c33-638f691dc5b6)

Nimble also has currency support. You are able to convert/transfer funds from either one of your wallets. If the user does not have a wallet for the currency you want to convert, Nimble will automatically create a new wallet of that currency. This logic is implemented in the Convert Funds page: 
![image](https://github.com/user-attachments/assets/f66428fe-8fa6-479b-bee1-f21dd9fbea92)


Another notable feature is Nimble's Savings Wallet. The user choses an amount and a duration for the wallet and is shown the interest rate they are going to receive. The user can review the Savings Wallet creation and confirm it or go back and edit the details. The interest rate should vary based on the duration and amount saved. Once the selected duration for the account passes, the saved amount plus the interest is automatically added to the wallet, from which they were taken from. The user has a page where they can view their Savings Wallets.
Create Savings Wallet page:
![image](https://github.com/user-attachments/assets/6bec0488-66d6-48b3-a113-6c83bb8a9e91)

Savings Wallet Details and Confirmation page:
![image](https://github.com/user-attachments/assets/a53f3e35-9de1-4d78-827e-34122b0fb97e)

To reach maximum security, the app has additional steps to go through if a transaction is large (>300 [of the current currency]). There is a confirmation page for the transaction's details and after it is confirmed by the user, an e-mail is being sent with a secret code. Simultaneously, the user is redirected to a page containing a form to input the secret code to confirm and complete the transaction.

Send Money: 
![image](https://github.com/user-attachments/assets/0be425ab-0490-466c-a236-342859f82188)

Confirmation and Details page: 
![image](https://github.com/user-attachments/assets/b204181e-1794-481e-bbee-b108a4f72306)

Large Transaction Verification page(if the amount is >300):
![image](https://github.com/user-attachments/assets/7c8db4d4-deb8-4f81-bb64-892a23315695)

E-mail with the secret code: 
![image](https://github.com/user-attachments/assets/7453debe-30a2-42bc-b9b8-008e566f38f8)

Success page if the secret code is valid:
![image](https://github.com/user-attachments/assets/5d44a1f5-0e97-4b17-9ee6-619dc1c79815)

Each user can access a list of each transaction made by him/her by clicking on "My Transactions": 
![image](https://github.com/user-attachments/assets/902a701a-7dd0-45dc-b9b3-dc3a3412ea3c)

The transactions can be filtered by recipient, amount, from date to date and by transaction type:
![image](https://github.com/user-attachments/assets/699dd90c-bb78-49d4-b38a-79eac18e2e1c)

All users also have a friends list where they can add and remove other users(used for fast access):
![image](https://github.com/user-attachments/assets/d1848111-9ef8-43ea-8536-3a2692c22212)


**Admin Features**
Admin users have all the described functionalities plus some extra ones.

Home page of admin user:
![image](https://github.com/user-attachments/assets/7d847428-d1a6-45cc-af37-747453c29e09)

Admins have access to all transactions made by every user. He can assign roles(assign other users as "Admins") and view all verification applies(selfie and ID verifications).

All Transactions page where filter/sort functions are implemented as well as pagination:
![image](https://github.com/user-attachments/assets/64b199a5-450c-448a-b2db-ed7593e2847a)

An admin can also view all users and search for a user by username, phone number and e-mail:
![image](https://github.com/user-attachments/assets/e1010874-ff17-466e-af9e-ebe51b2c684f)

All Verification Applies page (currently with one user waiting for veification): 
![image](https://github.com/user-attachments/assets/bdf98692-8c05-4e62-8655-912acb3cdcac)


**Admin users cannot send money**

## Features
- Send Money(wallet to wallet)
- Multiple Cards
- Multiple Wallets(one for each currency)
- Convert Funds
- Large tarnsaction verification
- Savings Wallet
- Identity verification
- Friends list
- Currency support
- Filter/Sort transactions

## Technologies
- ASP.NET Core
- Entity Framework Core
- SQL Server
-  CloudinaryDotNet
-  GitHub
-  external API
-  SMTP client
   


## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Database Diagram
![image](https://github.com/user-attachments/assets/c59555fc-54a7-4290-b841-308554aa8584)


## API Documentation

You can find the interactive API documentation at the link below:

[Swagger Documentation] - http://localhost:5000/swagger/index.html

## Contact
Alexander Georgiev - alexander.georgiev.a59@learn.telerikacademy.com
Samuil Milchev - samuil.milchev.a59@learn.telerikacademy.com
Violin Filev - violin.filev.a59@learn.telerikacademy.com
