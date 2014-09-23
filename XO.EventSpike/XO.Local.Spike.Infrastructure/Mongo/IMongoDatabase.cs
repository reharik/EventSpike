using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace XO.Local.Spike.Infrastructure.Mongo
{
    public interface IMongoDatabase
    {
        /// <summary>
        /// Gets the default GridFS instance for this database. The default GridFS instance uses default GridFS
        ///             settings. See also GetGridFS if you need to use GridFS with custom settings.
        /// 
        /// </summary>
        MongoGridFS GridFS { get; }

        /// <summary>
        /// Gets the name of this database.
        /// 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the server that contains this database.
        /// 
        /// </summary>
        MongoServer Server { get; }

        /// <summary>
        /// Gets the settings being used to access this database.
        /// 
        /// </summary>
        MongoDatabaseSettings Settings { get; }

        /// <summary>
        /// Tests whether a collection exists on this database.
        /// 
        /// </summary>
        /// <param name="collectionName">The name of the collection.</param>
        /// <returns>
        /// True if the collection exists.
        /// </returns>
        bool CollectionExists(string collectionName);

        /// <summary>
        /// Creates a collection. MongoDB creates collections automatically when they are first used, so
        ///             this command is mainly here for frameworks.
        /// 
        /// </summary>
        /// <param name="collectionName">The name of the collection.</param>
        /// <returns>
        /// A CommandResult.
        /// </returns>
        CommandResult CreateCollection(string collectionName);

        /// <summary>
        /// Creates a collection. MongoDB creates collections automatically when they are first used, so
        ///             you only need to call this method if you want to provide non-default options.
        /// 
        /// </summary>
        /// <param name="collectionName">The name of the collection.</param><param name="options">Options for creating this collection (usually a CollectionOptionsDocument or constructed using the CollectionOptions builder).</param>
        /// <returns>
        /// A CommandResult.
        /// </returns>
        CommandResult CreateCollection(string collectionName, IMongoCollectionOptions options);

        /// <summary>
        /// Drops a database.
        /// 
        /// </summary>
        void Drop();

        /// <summary>
        /// Drops a collection.
        /// 
        /// </summary>
        /// <param name="collectionName">The name of the collection to drop.</param>
        /// <returns>
        /// A CommandResult.
        /// </returns>
        CommandResult DropCollection(string collectionName);

        /// <summary>
        /// Evaluates JavaScript code at the server.
        /// 
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>
        /// The result of evaluating the code.
        /// </returns>
        BsonValue Eval(EvalArgs args);

        /// <summary>
        /// Fetches the document referred to by the DBRef.
        /// 
        /// </summary>
        /// <param name="dbRef">The <see cref="T:MongoDB.Driver.MongoDBRef"/> to fetch.</param>
        /// <returns>
        /// A BsonDocument (or null if the document was not found).
        /// </returns>
        BsonDocument FetchDBRef(MongoDBRef dbRef);

        /// <summary>
        /// Fetches the document referred to by the DBRef, deserialized as a <typeparamref name="TDocument"/>.
        /// 
        /// </summary>
        /// <typeparam name="TDocument">The nominal type of the document to fetch.</typeparam><param name="dbRef">The <see cref="T:MongoDB.Driver.MongoDBRef"/> to fetch.</param>
        /// <returns>
        /// A <typeparamref name="TDocument"/> (or null if the document was not found).
        /// </returns>
        TDocument FetchDBRefAs<TDocument>(MongoDBRef dbRef);

        /// <summary>
        /// Fetches the document referred to by the DBRef.
        /// 
        /// </summary>
        /// <param name="documentType">The nominal type of the document to fetch.</param><param name="dbRef">The <see cref="T:MongoDB.Driver.MongoDBRef"/> to fetch.</param>
        /// <returns>
        /// An instance of nominalType (or null if the document was not found).
        /// </returns>
        object FetchDBRefAs(Type documentType, MongoDBRef dbRef);

        /// <summary>
        /// Gets a MongoCollection instance representing a collection on this database
        ///             with a default document type of TDefaultDocument.
        /// 
        /// </summary>
        /// <typeparam name="TDefaultDocument">The default document type for this collection.</typeparam><param name="collectionName">The name of the collection.</param>
        /// <returns>
        /// An instance of MongoCollection.
        /// </returns>
        MongoCollection<TDefaultDocument> GetCollection<TDefaultDocument>(string collectionName);

        /// <summary>
        /// Gets a MongoCollection instance representing a collection on this database
        ///             with a default document type of TDefaultDocument.
        /// 
        /// </summary>
        /// <typeparam name="TDefaultDocument">The default document type for this collection.</typeparam><param name="collectionName">The name of the collection.</param><param name="collectionSettings">The settings to use when accessing this collection.</param>
        /// <returns>
        /// An instance of MongoCollection.
        /// </returns>
        MongoCollection<TDefaultDocument> GetCollection<TDefaultDocument>(string collectionName,
                                                                          MongoCollectionSettings collectionSettings);

        /// <summary>
        /// Gets a MongoCollection instance representing a collection on this database
        ///             with a default document type of TDefaultDocument.
        /// 
        /// </summary>
        /// <typeparam name="TDefaultDocument">The default document type for this collection.</typeparam><param name="collectionName">The name of the collection.</param><param name="writeConcern">The write concern to use when accessing this collection.</param>
        /// <returns>
        /// An instance of MongoCollection.
        /// </returns>
        MongoCollection<TDefaultDocument> GetCollection<TDefaultDocument>(string collectionName,
                                                                          WriteConcern writeConcern);

        /// <summary>
        /// Gets a MongoCollection instance representing a collection on this database
        ///             with a default document type of BsonDocument.
        /// 
        /// </summary>
        /// <param name="collectionName">The name of the collection.</param>
        /// <returns>
        /// An instance of MongoCollection.
        /// </returns>
        MongoCollection<BsonDocument> GetCollection(string collectionName);

        /// <summary>
        /// Gets a MongoCollection instance representing a collection on this database
        ///             with a default document type of TDefaultDocument.
        /// 
        /// </summary>
        /// <param name="collectionName">The name of the collection.</param><param name="collectionSettings">The settings to use when accessing this collection.</param>
        /// <returns>
        /// An instance of MongoCollection.
        /// </returns>
        MongoCollection<BsonDocument> GetCollection(string collectionName, MongoCollectionSettings collectionSettings);

        /// <summary>
        /// Gets a MongoCollection instance representing a collection on this database
        ///             with a default document type of BsonDocument.
        /// 
        /// </summary>
        /// <param name="collectionName">The name of the collection.</param><param name="writeConcern">The write concern to use when accessing this collection.</param>
        /// <returns>
        /// An instance of MongoCollection.
        /// </returns>
        MongoCollection<BsonDocument> GetCollection(string collectionName, WriteConcern writeConcern);

        /// <summary>
        /// Gets a MongoCollection instance representing a collection on this database
        ///             with a default document type of BsonDocument.
        /// 
        /// </summary>
        /// <param name="defaultDocumentType">The default document type.</param><param name="collectionName">The name of the collection.</param>
        /// <returns>
        /// An instance of MongoCollection.
        /// </returns>
        MongoCollection GetCollection(Type defaultDocumentType, string collectionName);

        /// <summary>
        /// Gets a MongoCollection instance representing a collection on this database
        ///             with a default document type of BsonDocument.
        /// 
        /// </summary>
        /// <param name="defaultDocumentType">The default document type.</param><param name="collectionName">The name of the collection.</param><param name="collectionSettings">The settings to use when accessing this collection.</param>
        /// <returns>
        /// An instance of MongoCollection.
        /// </returns>
        MongoCollection GetCollection(Type defaultDocumentType, string collectionName,
                                      MongoCollectionSettings collectionSettings);

        /// <summary>
        /// Gets a MongoCollection instance representing a collection on this database
        ///             with a default document type of BsonDocument.
        /// 
        /// </summary>
        /// <param name="defaultDocumentType">The default document type.</param><param name="collectionName">The name of the collection.</param><param name="writeConcern">The write concern to use when accessing this collection.</param>
        /// <returns>
        /// An instance of MongoCollection.
        /// </returns>
        MongoCollection GetCollection(Type defaultDocumentType, string collectionName, WriteConcern writeConcern);

        /// <summary>
        /// Gets a list of the names of all the collections in this database.
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// A list of collection names.
        /// </returns>
        IEnumerable<string> GetCollectionNames();

        /// <summary>
        /// Gets the current operation.
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// The current operation.
        /// </returns>
        BsonDocument GetCurrentOp();

        /// <summary>
        /// Gets an instance of MongoGridFS for this database using custom GridFS settings.
        /// 
        /// </summary>
        /// <param name="gridFSSettings">The GridFS settings to use.</param>
        /// <returns>
        /// An instance of MongoGridFS.
        /// </returns>
        MongoGridFS GetGridFS(MongoGridFSSettings gridFSSettings);

        /// <summary>
        /// Gets the last error (if any) that occurred on this connection. You MUST be within a RequestStart to call this method.
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// The last error (<see cref="T:MongoDB.Driver.GetLastErrorResult"/>)
        /// </returns>
        GetLastErrorResult GetLastError();

        /// <summary>
        /// Gets one or more documents from the system.profile collection.
        /// 
        /// </summary>
        /// <param name="query">A query to select which documents to return.</param>
        /// <returns>
        /// A cursor.
        /// </returns>
        MongoCursor<SystemProfileInfo> GetProfilingInfo(IMongoQuery query);

        /// <summary>
        /// Gets the current profiling level.
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// The profiling level.
        /// </returns>
        GetProfilingLevelResult GetProfilingLevel();

        /// <summary>
        /// Gets a sister database on the same server.
        /// 
        /// </summary>
        /// <param name="databaseName">The name of the sister database.</param>
        /// <returns>
        /// An instance of MongoDatabase.
        /// </returns>
        MongoDatabase GetSisterDatabase(string databaseName);

        /// <summary>
        /// Gets the current database stats.
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// An instance of DatabaseStatsResult.
        /// </returns>
        DatabaseStatsResult GetStats();

        /// <summary>
        /// Checks whether a given collection name is valid in this database.
        /// 
        /// </summary>
        /// <param name="collectionName">The collection name.</param><param name="message">An error message if the collection name is not valid.</param>
        /// <returns>
        /// True if the collection name is valid; otherwise, false.
        /// </returns>
        bool IsCollectionNameValid(string collectionName, out string message);

        /// <summary>
        /// Renames a collection on this database.
        /// 
        /// </summary>
        /// <param name="oldCollectionName">The old name for the collection.</param><param name="newCollectionName">The new name for the collection.</param>
        /// <returns>
        /// A CommandResult.
        /// </returns>
        CommandResult RenameCollection(string oldCollectionName, string newCollectionName);

        /// <summary>
        /// Renames a collection on this database.
        /// 
        /// </summary>
        /// <param name="oldCollectionName">The old name for the collection.</param><param name="newCollectionName">The new name for the collection.</param><param name="dropTarget">Whether to drop the target collection first if it already exists.</param>
        /// <returns>
        /// A CommandResult.
        /// </returns>
        CommandResult RenameCollection(string oldCollectionName, string newCollectionName, bool dropTarget);

        /// <summary>
        /// Lets the server know that this thread is done with a series of related operations. Instead of calling this method it is better
        ///             to put the return value of RequestStart in a using statement.
        /// 
        /// </summary>
        void RequestDone();

        /// <summary>
        /// Lets the server know that this thread is about to begin a series of related operations that must all occur
        ///             on the same connection. The return value of this method implements IDisposable and can be placed in a
        ///             using statement (in which case RequestDone will be called automatically when leaving the using statement).
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// A helper object that implements IDisposable and calls <see cref="M:MongoDB.Driver.MongoDatabase.RequestDone"/> from the Dispose method.
        /// </returns>
        IDisposable RequestStart();

        /// <summary>
        /// Lets the server know that this thread is about to begin a series of related operations that must all occur
        ///             on the same connection. The return value of this method implements IDisposable and can be placed in a
        ///             using statement (in which case RequestDone will be called automatically when leaving the using statement).
        /// 
        /// </summary>
        /// <param name="readPreference">The read preference.</param>
        /// <returns>
        /// A helper object that implements IDisposable and calls <see cref="M:MongoDB.Driver.MongoDatabase.RequestDone"/> from the Dispose method.
        /// </returns>
        IDisposable RequestStart(ReadPreference readPreference);

        /// <summary>
        /// Runs a command on this database.
        /// 
        /// </summary>
        /// <param name="command">The command object.</param>
        /// <returns>
        /// A CommandResult
        /// </returns>
        CommandResult RunCommand(IMongoCommand command);

        /// <summary>
        /// Runs a command on this database.
        /// 
        /// </summary>
        /// <param name="commandName">The name of the command.</param>
        /// <returns>
        /// A CommandResult
        /// </returns>
        CommandResult RunCommand(string commandName);

        /// <summary>
        /// Runs a command on this database and returns the result as a TCommandResult.
        /// 
        /// </summary>
        /// <typeparam name="TCommandResult">The type of the returned command result.</typeparam><param name="command">The command object.</param>
        /// <returns>
        /// A TCommandResult
        /// </returns>
        TCommandResult RunCommandAs<TCommandResult>(IMongoCommand command) where TCommandResult : CommandResult;

        /// <summary>
        /// Runs a command on this database and returns the result as a TCommandResult.
        /// 
        /// </summary>
        /// <typeparam name="TCommandResult">The type of the returned command result.</typeparam><param name="commandName">The name of the command.</param>
        /// <returns>
        /// A TCommandResult
        /// </returns>
        TCommandResult RunCommandAs<TCommandResult>(string commandName) where TCommandResult : CommandResult;

        /// <summary>
        /// Runs a command on this database and returns the result as a TCommandResult.
        /// 
        /// </summary>
        /// <param name="commandResultType">The command result type.</param><param name="command">The command object.</param>
        /// <returns>
        /// A TCommandResult
        /// </returns>
        CommandResult RunCommandAs(Type commandResultType, IMongoCommand command);

        /// <summary>
        /// Runs a command on this database and returns the result as a TCommandResult.
        /// 
        /// </summary>
        /// <param name="commandResultType">The command result type.</param><param name="commandName">The name of the command.</param>
        /// <returns>
        /// A TCommandResult
        /// </returns>
        CommandResult RunCommandAs(Type commandResultType, string commandName);

        /// <summary>
        /// Sets the level of profile information to write.
        /// 
        /// </summary>
        /// <param name="level">The profiling level.</param>
        /// <returns>
        /// A CommandResult.
        /// </returns>
        CommandResult SetProfilingLevel(ProfilingLevel level);

        /// <summary>
        /// Sets the level of profile information to write.
        /// 
        /// </summary>
        /// <param name="level">The profiling level.</param><param name="slow">The threshold that defines a slow query.</param>
        /// <returns>
        /// A CommandResult.
        /// </returns>
        CommandResult SetProfilingLevel(ProfilingLevel level, TimeSpan slow);

        /// <summary>
        /// Gets a canonical string representation for this database.
        /// 
        /// </summary>
        /// 
        /// <returns>
        /// A canonical string representation for this database.
        /// </returns>
        string ToString();
    }
}