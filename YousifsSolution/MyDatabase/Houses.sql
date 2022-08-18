CREATE TABLE [dbo].[Houses]
(
	[Id] INT NOT NULL identity PRIMARY KEY, 
    [Address] NVARCHAR(50) unique NOT NULL, 
    [Color] NVARCHAR(7) NOT NULL, 
    [RoofID] INT NOT NULL, 
    [HaveBalcony] BIT NOT NULL, 
    [HaveWindow] BIT NOT NULL, 
    [HaveDoor] BIT NOT NULL, 
    [NumberOfFloors] INT NOT NULL
    foreign key (RoofID) REFERENCES [dbo].[Roofs] ([Id]), 
    [SortingOrder] INT NOT NULL, 
    [Width] INT NOT NULL  
)
