using System.Collections.Generic;
using Astraea.Inframap.Data;
using TopCoder.Configuration;
using System.Data;
using TopCoder.Data.ConnectionFactory;
using System;
namespace Astraea.Inframap.Downloader.Persistence
{

    ///  <summary>
    ///  <p>Implementation of the IMapDataRetriever interface to retrieve map data, styles, and attribute and element types. This implementation is back by a SQL Server 2005 database. All calls are performed using a connection obtained from the ConnectionManager. Configuration of the retriever is done using the Configuration API.</p>
    ///  <p>Thread Safety: This class is immutable and thread-safe.</p>           ///  </summary>
    public class SqlServerMapDataRetriever : IMapDataRetriever
    {
        /// <summary>
        /// Represents the name of the connection to obtain from the ConnectionManager in all operations. This is obtained in the constructor, and will never change or be null/empty.             /// </summary>
        private readonly string connectionName;

        /// <summary>
        /// Represents the mappings of configured table names. This is obtained in the constructor, and will never change or be null/empty. In fact it will have 9 entries.             /// </summary>
        private readonly IDictionary<string, string> tableNames = new Dictionary<string, string>();

        /// Attribute AttributesTableNameKey
        /// <summary>
        /// Represents the key to the dictionary for the attributes table name.
        /// </summary>
        public const string AttributesTableNameKey = "attributes";

        /// Attribute AttributeTypesTableNameKey
        /// <summary>
        /// Represents the key to the dictionary for the attribute types table name.
        /// </summary>
        public string AttributeTypesTableNameKey = "attributeTypes";

        /// Attribute StylesTableNameKey
        /// <summary>
        /// Represents the key to the dictionary for the styles table name.
        /// </summary>
        public const string StylesTableNameKey = "styles";

        /// Attribute ElementTypesTableNameKey
        /// <summary>
        /// Represents the key to the dictionary for the element types table name.
        /// </summary>
        public string ElementTypesTableNameKey = "elementTypes";

        /// Attribute LinksTableNameKey
        /// <summary>
        /// Represents the key to the dictionary for the links table name.
        /// </summary>
        public const string LinksTableNameKey = "links";

        /// Attribute NodesTableNameKey
        /// <summary>
        /// Represents the key to the dictionary for the maps table name.
        /// </summary>
        public const string NodesTableNameKey = "maps";

        /// Attribute MapsTableNameKey
        /// <summary>
        /// Represents the key to the dictionary for the nodes table name.
        /// </summary>
        public const string MapsTableNameKey = "nodes";

        /// Attribute PlugsTableNameKey
        /// <summary>
        /// Represents the key to the dictionary for the plugs table name.
        /// </summary>
        public const string PlugsTableNameKey = "plugs";

        /// Attribute PortsTableNameKey
        /// <summary>
        /// Represents the key to the dictionary for the ports table name.
        /// </summary>
        public const string PortsTableNameKey = "ports";

        /// Attribute DefaultAttributesTableName
        /// <summary>
        /// Represents default name of the attributes table.
        /// </summary>
        public const string DefaultAttributesTableName = "attribs";

        /// Attribute DefaultAttributeTypesTableName
        /// <summary>
        /// Represents default name of the attribute types table.
        /// </summary>
        public string DefaultAttributeTypesTableName = "attribute_types";

        /// Attribute DefaultStylesTableName
        /// <summary>
        /// Represents default name of the styles table.
        /// </summary>
        public const string DefaultStylesTableName = "element_type_styles";

        /// Attribute DefaultElementTypesTableName
        /// <summary>
        /// Represents default name of the element types table.
        /// </summary>
        public const string DefaultElementTypesTableName = "element_types";

        /// Attribute DefaultLinksTableName
        /// <summary>
        /// Represents default name of the links table.
        /// </summary>
        public const string DefaultLinksTableName = "links";

        /// Attribute DefaultNodesTableName
        /// <summary>
        /// Represents default name of the maps table.
        /// </summary>
        public const string DefaultNodesTableName = "maps";

        /// Attribute DefaultMapsTableName
        /// <summary>
        /// Represents default name of the nodes table.
        /// </summary>
        public const string DefaultMapsTableName = "nodes";

        /// Attribute DefaultPlugsTableName
        /// <summary>
        /// Represents default name of the plugs table.
        /// </summary>
        public const string DefaultPlugsTableName = "plugs";

        /// Attribute DefaultPortsTableName
        /// <summary>
        /// Represents default name of the ports table.
        /// </summary>
        public const string DefaultPortsTableName = "ports";

        private readonly string queryTextElementTypes;

        /// Constructor SqlServerMapDataRetriever
        /// <summary>
        /// Constructor: Obtains the name of the connection from The IConfiguration from attribute &quot;connectionName&quot;, and names of tables. 
        /// <p>Add all names to the internal dictionary. The properties in configuration have the same name as the provided keys, except they are prefixed by &quot;table_&quot;.
        /// For all tables whose names are not given, set them to the default values.</p>             /// </summary>
        /// <exception>ArgumentNullException If configuration is null</exception>
        /// <exception>ArgumentException If configuration is missing required parameters</exception>
        /// <param name='configuration'>The configuration object with the connection name</param>
        public SqlServerMapDataRetriever(IConfiguration configuration)
        {
            tableNames[ElementTypesTableNameKey] = DefaultElementTypesTableName;
            queryTextElementTypes = "select * from " + tableNames[ElementTypesTableNameKey];
        }

        /// Constructor SqlServerMapDataRetriever
        /// <summary>
        /// Constructor: Sets the passed parameters to the fields. The dictionary is copied to the internal dictionary.
        /// Check if every TableKey is available, and set the ones that are not to the default value.
        /// </summary>
        /// <exception>ArgumentNullException If connectionName or tableNames is null</exception>
        /// <exception>ArgumentException If connectionName is empty</exception>
        /// <param name='connectionName'>the name of the connection to obtain from the ConnectionManager in all operations</param>
        /// <param name='tableNames'>IDictionary[string,string] of configured table names</param>
        public SqlServerMapDataRetriever(string connectionName, IDictionary<string, string> tableNames)
        {
            this.connectionName = connectionName;

            tableNames[ElementTypesTableNameKey] = DefaultElementTypesTableName;
            queryTextElementTypes = "select * from " + tableNames[ElementTypesTableNameKey];
        }

        /// Operation DownloadMapData
        /// <summary>
        /// Gets the map data with the given ID.
        /// <p>The actual table names used in the queries are obtained from the tableNames dictionary.</p>
        /// <p>Note that almost all table columns have the same names as entity properties, except for thew following exceptions:</p>
        /// <ul type="disc">
        /// <li>attribs.owner_id: MapAttribute.OwnerId</li>
        /// <li>attribs.owner_type: MapAttribute.OwnerType</li>
        /// <li>attribs.v_int: MapAttribute.IntValue</li>
        /// <li>attribs.v_str: MapAttribute.StringValue</li>
        /// <li>attribs.v_tds: MapAttribute.DateTimeValue</li>
        /// <li>attribs.v_dc6: MapAttribute.DoubleValue</li>
        /// </ul>
        /// <p><strong>Implementation Notes</strong></p>
        /// <ul type="disc">
        /// <li>Get instance of IDbConnection from ConnectionManager with&nbsp;this.connectionName</li>
        /// <li>Prepare a dictionary named elementTypeMappings: it will be used to map MapElementTypes back to their MapElements.
        ///     Each key will be the elementTypeId, and the value will be a list of MapElement</li>
        /// <li>
        /// Perform query to get the maps record for the pass mapId
        /// <ul type="disc">
        /// <li>Construct MapData from it</li>
        /// <li>Construct a MapHeader with Lifecycle = &quot;map_data&quot;, Version = &quot;1.0&quot;, and TimeStamp of now, and add to the MapData</li>
        /// <li>Call DownloadAttributeTypes and DownloadElementTypes and set these in the MapData.</li>
        /// </ul>
        /// </li>
        /// <li>
        /// Perform query to get all links for this mapId
        /// <ul type="disc">
        /// <li>For each link, create a new MapLink&nbsp; and fill with data from the row</li>
        /// <li>Put all MapLinks into a list and set in the MapData</li>
        /// <li>Put the links into elementTypeMappings dictionary</li>
        /// </ul>
        /// </li>
        /// <li>
        /// Perform query to get all ports for this mapId
        /// <ul type="disc">
        /// <li>For each port, create a new MapPort&nbsp; and fill with data from the row</li>
        /// <li>Put all ports with a node (stub_id) into a dictionary called portNodeMappings where the nodeId is the key and value is a list of MapPorts</li>
        /// <li>Put all MapPorts into a list and set in the MapData</li>
        /// <li>Put the ports into elementTypeMappings dictionary</li>
        /// </ul>
        /// </li>
        /// <li>
        /// Perform query to get all nodes for this mapId
        /// <ul type="disc">
        /// <li>For each node, create a new MapNode&nbsp; and fill with data from the row. Add an empty MapLabel in its Label property.</li>
        /// <li>Put all MapNodes into a list and set in the MapData</li>
        /// <li>
        /// Put all nodes with a parent (container_id) into a dictionary called containers where the containerId is the key and value is a list of MapNodes
        /// <ul type="disc">
        /// <li>All nodes wihtout a parent are put in a list of MapNodes called roots</li>
        /// </ul>
        /// </li>
        /// <li>Put all nodes with a stub (stub_id) into a dictionary called stubs where the stubId is the key and value is a list of MapNodes</li>
        /// <li>Put the nodes into elementTypeMappings dictionary</li>
        /// </ul>
        /// </li>
        /// <li>
        /// Perform query to get all element types whose id appeared in the queries for the links, ports, and nodes
        /// <ul type="disc">
        /// <li>For each retrieved record, construct a new MapElementType and fill it with the row info, and set it to the appropriate MapLink, MapPort, and MapNode. This is done by referring to the elementTypes dictionary.</li>
        /// </ul>
        /// </li>
        /// <li>
        /// Perform query to get all attributes whose owner_id is in the set of links, ports, nodes, or elementTypes ids, sorted by those IDs. and whose types is not &quot;AttributeTxt&quot; or &quot;AttributeBin&quot;
        /// <ul type="disc">
        /// <li>For each retrieved record, construct a new MapAttribute and fill it with the row info, and set it to the appropriate MapLink, MapPort, MapNode, or MapElementType. Since the MapAttribute are sorted, once each destination is found, it can receive all its MapAttributes</li>
        /// <li>Set all these attributes (except for the ones in the element types) in a list and set to MapData.</li>
        /// </ul>
        /// </li>
        /// <li>Perform the attribute override in each MapElement: If the MapElement's MapElementType contains an attribute it does not have (by name), it adds it to its own attributes list.</li>
        /// <li>Attach nodes via their container Id: Use the containers dictionary for this, starting with the roots list and performing a BFS. Remember to set the children of the parent in the parent's ContaindedNodes list too.</li>
        /// <li>Attach nodes via their stubId and ports via the nodeId: Iterate over each node checking if its ID has an entry in the stobs dictionary. If so, it will add itself to each MapNode in the value list as a stub. Then check if it has an entry in the portNodeMappings dicttionary, and put all the MapPorts into the node (and the node in each MapPort)</li>
        /// <li>Get all plugs for every retrieved link. Use this to match the link with the ports. This relationship is bi-directional, where eadh link has many ports. Then add each node of that port to the link.</li>
        /// </ul>
        /// <p></p>
        /// <ul type="disc">
        /// <li>Close connection and return MapData</li>
        /// </ul>             /// </summary>
        /// <exception>MapIdNotFoundException If there is no MapData with the given ID</exception>
        /// <exception>MapDataSourceException If there were errors during this operation</exception>
        /// <param name='mapId'>The ID of the MapData to be retrieved</param>
        /// <returns>Retrieved MapData</returns>
        public MapData DownloadMapData(long mapId)
        {

            // your code here
            return null;

        }

        /// Operation DownloadStyles
        /// <summary>
        /// Gets all styles of the map data with the given ID. Returns an empty list if none found. It will never return null or a list with null elements.
        /// <p>The actual table names used in the queries are obtained from the tableNames dictionary.</p>
        /// <p><strong>Implementation Notes</strong></p>
        /// <ul type="disc">
        /// <li>Get instance of IDbConnection from ConnectionManager with&nbsp;this.connectionName</li>
        /// <li>
        /// Perform query to obtain all styles table records for the given map:
        /// <ul type="disc">
        /// <li>
        /// to get all styles, we need to have a union of several select statements:
        /// <ul type="disc">
        /// <li>SELECT all styles records WHERE maps links to links via map_id AND links links to elementTypes by element_type_id AND elementTypes links to styles by element_type_id AND mapId = passed mapId</li>
        /// <li>SELECT all styles records WHERE maps links to ports via map_id AND ports links to elementTypes by element_type_id AND elementTypes links to styles by element_type_id AND mapId = passed mapId</li>
        /// <li>SELECT all styles records WHERE maps links to nodes via map_id AND nodes links to elementTypes by element_type_id AND elementTypes links to styles by element_type_id AND mapId = passed mapId</li>
        /// </ul>
        /// </li>
        /// <li>SELECT all distinct styles table records FROM the above union</li>
        /// </ul>
        /// </li>
        /// <li>For each retrieved record, construct a new MapStyle and fill it with the row info.</li>
        /// <li>Close connection and return the list of the MapStyles</li>
        /// </ul>             /// </summary>
        /// <exception>MapIdNotFoundException If there is no MapData with the given ID</exception>
        /// <exception>MapDataSourceException If there were errors during this operation</exception>
        /// <param name='mapId'>The ID of the MapData whose styles are to be retrieved</param>
        /// <returns>List of MapStyles of the map data with the given ID</returns>
        public IList<MapStyle> DownloadStyles(long mapId)
        {
            IList<MapStyle> result = null;

            using (IDbConnection connection = CreateConnection("Sql1"))
            {
                connection.Open();

                using (IDataReader reader = Helper.ExecuteReader(connection, queryTextElementTypes))
                {
                }

                return result;
            }
        }

        /// Operation DownloadAttributeTypes
        /// <summary>
        /// Gets all available MapAttributeTypes. Returns an empty list if none found. It will never return null or a list with null elements.
        /// <p>The actual table names used in the queries are obtained from the tableNames dictionary.</p>
        /// <p><strong>Implementation Notes</strong></p>
        /// <ul type="disc">
        /// <li>Get instance of IDbConnection from ConnectionManager with&nbsp;this.connectionName</li>
        /// <li>Perform query to obtain all records from attributeTypes and elementTypes WHERE they are joined on the element_type_id</li>
        /// <li>For each retrieved record, construct a new MapAttributeType and MapElementType and fill them with the row info. Then put the MapElementType into the MapAttributeType.</li>
        /// <li>Perform query to obtain all rows from attributes whose owner_id is in the set of elementType IDs</li>
        /// <li>For each retrieved record, assemble it into a MapAttribute and add to the appropriate MapElementType</li>
        /// <li>Close connection and return the list of available MapAttributeTypes</li>
        /// </ul>             /// </summary>
        /// <exception>MapDataSourceException If there were errors during this operation</exception>
        /// <returns>List of available MapAttributeTypes</returns>
        public IList<MapAttributeType> DownloadAttributeTypes()
        {
            IList<MapAttributeType> result = null;
            MapAttributeType attrType = null;
            IDictionary<long, IList<MapAttribute>> attrsDict = null;
            IList<MapAttribute> attrs = null;
            MapAttribute attr = null;
            //using (IDbConnection connection = CreateConnection("Sql1"))
            //{
            //    connection.Open();

            //    using (IDataReader reader = Helper.ExecuteReader(connection, queryTextElementTypes))
            //    {
            //        result = new List<MapElementType>();
            //        while (reader.Read())
            //        {
            //            elemType = new MapElementType();
            //            elemType.Id = reader.GetInt32(0);
            //            elemType.Name = reader.GetString(1);
            //            elemType.Layer = reader.GetInt32(2);

            //            result.Add(elemType);
            //        }
            //    }
            //    attrsDict = new Dictionary<long, IList<MapAttribute>>();
            //    foreach (MapElementType type in result)
            //    {
            //        if (attrsDict.ContainsKey(type.Id))
            //        {
            //            type.Attributes = attrsDict[type.Id];
            //        }
            //        else
            //        {
            //            using (IDataReader reader = Helper.ExecuteReader(connection, "SELECT * FROM attribs where owner_id = " + type.Id))
            //            {
            //                attrs = new List<MapAttribute>();
            //                while (reader.Read())
            //                {
            //                    attr = new MapAttribute();
            //                    attr.Id = reader.GetInt32(0);
            //                    attr.OwnerId = reader.GetInt32(1);
            //                    attr.OwnerType = reader.GetString(2);
            //                    attr.Type = reader.GetString(3);
            //                    attr.Name = reader.GetString(4);
            //                    attr.IntValue = reader.GetInt32(5);
            //                    attr.StringValue = reader.GetValue(6) as string;

            //                }
            //                type.Attributes = attrs;
            //                attrsDict[type.Id] = attrs;
            //            }

            //        }
            //    }
            //}


            // your code here
            return result;
        }

        /// <summary>
        /// Gets all available MapElementTypes. Returns an empty list if none found. It will never return null or a list with null elements.
        /// <p>The actual table names used in the queries are obtained from the tableNames dictionary.</p>
        /// <p><strong>Implementation Notes</strong></p>
        /// <ul type="disc">
        /// <li>Get instance of IDbConnection from ConnectionManager with&nbsp;this.connectionName</li>
        /// <li>Perform query to obtain all records from elementTypes</li>
        /// <li>For each retrieved record, construct a new MapElementType and fill it with the row info</li>
        /// <li>Perform query to obtain all rows from attributes whose owner_id is in the set of elementType IDs</li>
        /// <li>For each retrieved record, assemble it into a MapAttribute and add to the appropriate MapElementType</li>
        /// <li>Close connection and return the list of available MapElementTypes</li>
        /// </ul>             /// </summary>
        /// <exception>MapDataSourceException If there were errors during this operation</exception>
        /// <returns>List of available MapElementTypes</returns>
        public IList<MapElementType> DownloadElementTypes()
        {
            IList<MapElementType> result = null;
            MapElementType elemType = null;
            IDictionary<long, IList<MapAttribute>> attrsDict = null;
            IList<MapAttribute> attrs = null;
            MapAttribute attr = null;
            using (IDbConnection connection = CreateConnection("Sql1"))
            {
                connection.Open();

                using (IDataReader reader = Helper.ExecuteReader(connection, queryTextElementTypes))
                {
                    result = new List<MapElementType>();
                    while (reader.Read())
                    {
                        elemType = new MapElementType();
                        elemType.Id = reader.GetInt32(0);
                        elemType.Name = reader.GetString(1);
                        elemType.Layer = reader.GetInt32(2);

                        result.Add(elemType);
                    }
                }
                attrsDict = new Dictionary<long, IList<MapAttribute>>();
                foreach (MapElementType type in result)
                {
                    if (attrsDict.ContainsKey(type.Id))
                    {
                        type.Attributes = attrsDict[type.Id];
                    }
                    else
                    {
                        using (IDataReader reader = Helper.ExecuteReader(connection, "SELECT * FROM attribs where owner_id = " + type.Id))
                        {
                            attrs = new List<MapAttribute>();
                            while (reader.Read())
                            {
                                attr = new MapAttribute();
                                attr.Id = reader.GetInt32(0);
                                attr.OwnerId = reader.GetInt32(1);
                                attr.OwnerType = reader.GetString(2);
                                attr.Type = reader.GetString(3);
                                attr.Name = reader.GetString(4);
                                attr.IntValue = reader.GetInt32(5);
                                attr.StringValue = reader.GetValue(6) as string;

                            }
                            type.Attributes = attrs;
                            attrsDict[type.Id] = attrs;
                        }

                    }
                }
            }
            

            // your code here
            return result;

        }

        /// <summary>
        /// <para>
        /// Create a connection according the connection name.
        /// </para>
        /// </summary>
        /// <param name="connectionName">
        /// The connection name.
        /// </param>
        ///
        /// <returns>
        /// The <see cref="IDbConnection"/> created.
        /// </returns>
        private static IDbConnection CreateConnection(string connectionName)
        {
            return ConnectionManager.Instance.CreatePredefinedDbConnection(connectionName);
        }
    }
}