# Orleans Prototype

This is a sample app to test the performance of Orleans using table storage.

# Project summary
A user is allowed to create multiple puzzle pieces.
A puzzle piece is belongs to a puzzle piece definition.
We have a dispenser for each puzzle piece definition to make sure that unique puzzle pieces are handeded out.

The UserWalletGrain 
- gets a order command.
- Asks the PuzzlePieceDispenserGrain for x amout of puzzle pieces. (In the demo we only request 1 at a time)
- We then create an OrderGrain and proxy the order command to it
	- The OrderGrain then queries the appropriate PuzzlePiece and PuzzlePieceDefinition grain for some meta data before updating the state

As test data we create
* 1,000 PuzzlePieceDefinitionGrains
* 15 PuzzleDefinition per definition (hence 15000 in total)
* 100 UserWallets

We created 2 endpoints
/Home/WakeUp that creates all the above grains in at advance
/Home/Order that creates 10,000 orders by round robinning over the users and definitions (trying not to overload a specific grain).
