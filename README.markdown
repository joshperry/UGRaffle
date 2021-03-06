When a room full of software developers meets monthly for skill sharpening and learning you have to wonder why they draw for giveaway items by hand; passing out paper to write your name on, passing them back, put in a box/hat/cup/available-container, held comically high for random's sake.

This application begun as a talk that I presented to the [Utah .NET Users Group](http://utahdnug.org/) on MVC and NStack integration. Hopefully it'll mature enough that we can resume the laziness that us nerdy developers are known for, registering for the meeting and raffle items from a chair without any more human interaction than necessary.

Here's a list of initial "User Stories" that describe how I think the system should work:


##Accounts
+ Users can create an account with name, email, and password
+ Users can authenticate with email and password
+ Users can be administrators

##Admin
+ Administrators can create meetings with a description, date, and number of tickets awarded for attending
+ Administrators can add raffle items to a meeting with a Description, Image, and minimum ticket count

##Registration
+ Users can register as an attendee for a meeting
+ When a user registers they should have the ticket award for the meeting added to their cumulative ticket count
+ Users can view the current meeting and the raffle items available for that meeting and the number of tickets they have accrued
+ Users can create entries for a raffle item with as many tickets as they’d like up to the number of tickets they have acquired by attending meetings.

##Raffle
+ Administrators can run the raffle
+ The raffle process should present each item sequentially starting with the lowest required tickets moving to the highest
+ The raffle process should choose a winner by random selection from all available tickets in a stylin’ manner
+ All tickets allocated as entries by the winning user to the raffled item are forfeit
+ All tickets allocated as entries to the raffled item by non-winning users should be returned to the user
