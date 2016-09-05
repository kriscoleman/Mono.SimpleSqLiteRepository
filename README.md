# Mono.SimpleSqLiteRepository
A Simple SQLite Repository for Mono Projects

I designed this with this in mind: I want to declare POCOs, and be able to store them easily in a mobile database. 
I didn't like EF because it didn't create true POCOs, but I ended up abandoning this project when Realm was announced. 
I wanted to share it, and I might work on it more in the future. 

-Things to come: 
1) Creation of SQL CRUD generator - this will elimnate the CRUD objects from the framework, leaving declaration of POCOs all that is needed to store in repos. 
2) Schema-Diff logic to update the DB w/ new versions of your DB. 

-Express Feature: 
Implement ISqLiteDataObjects to immediately begin storing data objects in the default repo. 

-Advanced Feature: 
Derive new SimpleSqLiteRepositories to create many seperate repos. 
You can scope type-safety for each repro (to prevent saving data objects to the wrong repo, the compiler will prohibit you).
To scope type-safety, derive a new base class from ISqLiteDataObject and scope the repo to that abstract class 
by using the "new" keyword on all CRUD methods in your derived SimpleSqLiteRepository concrete.
This can for instance allow you to create a repo for MetaData, another for UserData, and another for AppData in three sepereate repositories. 
