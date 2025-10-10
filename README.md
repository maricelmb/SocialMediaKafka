o	Enables users to create, edit, and delete their own posts. Users can also add, edit, and delete their own comments. 
Additionally, the system supports querying all posts or filtering by ID or author, as well as retrieving posts along with their associated comments and likes.

o	It includes two RESTful APIs built with .NET: one for handling posts and another for queries. 
The posting API uses an Event-Driven Architecture, implementing the CQRS and Event Sourcing patterns to store events in MongoDB, 
publish them to Kafka, and have a consumer persist the necessary entities for querying in SQL Server. All query operations retrieve data directly from SQL Server.  
Kafka, SQL Server and MongoDB are all set up in Docker containers.
