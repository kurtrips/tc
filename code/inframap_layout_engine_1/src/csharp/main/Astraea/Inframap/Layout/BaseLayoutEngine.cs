/*
* Copyright (c) 2007, TopCoder, Inc. All rights reserved.
*/ 
using System;
using System.Collections;
using System.Collections.Generic;
using TopCoder.LoggingWrapper;
using TopCoder.Configuration;
using TopCoder.Graph.Layout;
using TopCoder.Util.ObjectFactory;
using TopCoder.Util.ExceptionManager.SDE;
using Astraea.Inframap.Data;

namespace Astraea.Inframap.Layout
{
    /// <summary>
    /// <para>
    /// This is a base (abstract) implementation for a layout engine which utilizes
    /// the IGraphLayouter for the actual processing and which exposes the pre and post processing
    /// template methods to be implemented for specific layouting actions.</para>
    /// <remarks>
    /// Please note that the object is fully configurable as well
    /// with specific read-only configuration data used in processing the layouting request.
    /// </remarks>
    /// </summary>
    ///
    /// <threadsafety>
    /// The current implementation is thread safe as it is immutable.
    /// </threadsafety>
    ///
    /// <author>AleaActaEst</author>
    /// <author>TCSDEVELOPER</author>
    /// <version>1.0</version>
    /// <copyright>Copyright (c) 2007, TopCoder, Inc. All rights reserved.</copyright>
    public abstract class BaseLayoutEngine : ILayoutEngine
    {
        /// <summary>
        /// The Lifecycle to set of the Mapheader of the MapData after the layout.
        /// </summary>
        private const string LIFECYCLE_LAYOUT = "layout";

        /// <summary>
        /// The configuration property for the default font size.
        /// </summary>
        private const string DEFAULT_FONT_SIZE = "default_font_size";

        /// <summary>
        /// The configuration property for the font units.
        /// </summary>
        private const string FONT_UNITS = "font_units";

        /// <summary>
        /// The configuration property for character units
        /// </summary>
        private const string CHARACTER_UNITS = "character_units";

        /// <summary>
        /// The configuration property for minimum port width.
        /// </summary>
        private const string MINIMUM_PORT_WIDTH = "minimum_port_width";

        /// <summary>
        /// The configuration property for minimum port height.
        /// </summary>
        private const string MINIMUM_PORT_HEIGHT = "minimum_port_height";

        /// <summary>
        /// The configuration property for minimum link space.
        /// </summary>
        private const string MINIMUM_LINK_SPACE = "minimum_link_space";

        /// <summary>
        /// The configuration property for minimum node width.
        /// </summary>
        private const string MINIMUM_NODE_WIDTH = "minimum_node_width";

        /// <summary>
        /// The configuration property for minimum node height.
        /// </summary>
        private const string MINIMUM_NODE_HEIGHT = "minimum_node_height";

        /// <summary>
        /// The configuration property for minimum node width.
        /// </summary>
        private const string MINIMUM_SYNTHETIC_NODE_WIDTH = "minimum_synthetic_node_width";

        /// <summary>
        /// The configuration property for minimum synthetic node height.
        /// </summary>
        private const string MINIMUM_SYNTHETIC_NODE_HEIGHT = "minimum_synthetic_node_height";

        /// <summary>
        /// The configuration property for minimum unlinked port space.
        /// </summary>
        private const string MINIMUM_UNLINKED_PORT_SPACE = "minimum_unlinked_port_space";

        /// <summary>
        /// The configuration property for the logger to use.
        /// </summary>
        private const string LOGGER_NAME = "logger_name";

        /// <summary>
        /// The configuration property for the graph layouter key.
        /// </summary>
        private const string GRAPH_LAYOUTER_TOKEN = "graph_layouter_token";

        /// <summary>
        /// The configuration property for object factory namespace.
        /// </summary>
        private const string OBJECT_FACTORY_NS = "object_factory_ns";

        /// <summary>
        /// <para>This is the input configuration object that holds the configuration
        /// data for this class.</para>
        /// <para>
        /// It cannot be null.
        /// Initialized in the constructor (deep copy through ICloneable) and becomes immutable.
        /// User cannot change anything about this but they can read the configuration object through a getter.
        /// It is utilized through specific property getter used to expose the configured properties.
        /// </para>
        /// </summary>
        private readonly IConfiguration config;

        /// <summary>
        /// <para>This is the graph layouter that is used by this base class to
        /// process the layouting of a MapData instance.</para>
        /// <para>
        /// Cannot be null.
        /// Initialized in the constructor through Object factory configuration token and becomes immutable.
        /// User cannot change anything about this but they can read the graph layouter object through a
        /// getter. It is utilized in the ProcessLayouting method.</para>
        /// </summary>
        private readonly IGraphLayouter graphLayouter;

        /// <summary>
        /// <para>This is optional logger to be utilized by this comonent. It is used to
        /// log actions/warnings/exceptions.</para>
        /// <para>
        /// Can be null.
        /// Initialized in the constructor through LogManager configured logger name and becomes immutable.
        /// User cannot change anything about this but they can read the it through a getter.
        /// It is utilized in all the implemented methods.
        /// </para>
        /// </summary>
        private readonly Logger logger;

        /// <summary>
        /// <para>
        /// Getter which returns the currently stored value under the 'default_font_size' in the configuration
        /// </para>
        /// </summary>
        /// <value>The default font size to be used.</value>
        public double DefaultFontSize
        {
            get
            {
                return Helper.ReadConfigDouble(config, DEFAULT_FONT_SIZE);
            }
        }

        /// <summary>
        /// <para>Getter which returns the currently stored value under the 'font_units' in the configuration</para>
        /// </summary>
        /// <value>
        /// One unit font size equal to how many layout units relation,
        /// for example if this is set to 3, and font size is 6, then the label height will be 3*6 units.
        /// </value>
        public double FontUnits
        {
            get
            {
                return Helper.ReadConfigDouble(config, FONT_UNITS);
            }
        }

        /// <summary>
        /// <para>
        /// Getter which returns the currently stored value under the 'character_units' in the configuration
        /// </para>
        /// </summary>
        /// <value>
        /// How many units for a character, for example if this is set to 2,
        /// and label length is 10, then the label width will be 2*10 units.
        /// </value>
        public double CharacterUnits
        {
            get
            {
                return Helper.ReadConfigDouble(config, CHARACTER_UNITS);
            }
        }

        /// <summary>
        /// <para>
        /// Getter which returns the currently stored value under the 'minimum_port_width' in the configuration.
        /// </para>
        /// </summary>
        /// <value>
        /// Minimum port size (width).
        /// </value>
        public int MinimumPortWidth
        {
            get
            {
                return Helper.ReadConfigInt(config, MINIMUM_PORT_WIDTH);
            }
        }

        /// <summary>
        /// <para>
        /// Getter which returns the currently stored value under the 'minimum_port_height' in the configuration.
        /// </para>
        /// </summary>
        /// <value>
        /// Minimum port size (height).
        /// </value>
        public int MinimumPortHeight
        {
            get
            {
                return Helper.ReadConfigInt(config, MINIMUM_PORT_HEIGHT);
            }
        }

        /// <summary>
        /// <para>
        /// Getter which returns the currently stored value under the 'minimum_link_space' in the configuration.
        /// </para>
        /// </summary>
        /// <value>
        /// Minimum space between links.
        /// </value>
        public int MinimumLinkSpace
        {
            get
            {
                return Helper.ReadConfigInt(config, MINIMUM_LINK_SPACE);
            }
        }

        /// <summary>
        /// <para>
        /// Getter which returns the currently stored value under the 'minimum_node_width' in the configuration
        /// </para>
        /// </summary>
        /// <value>
        /// Minimum node size (width).
        /// </value>
        public int MinimumNodeWidth
        {
            get
            {
                return Helper.ReadConfigInt(config, MINIMUM_NODE_WIDTH);
            }
        }

        /// <summary>
        /// <para>
        /// Getter which returns the currently stored value under the 'minimum_node_height' in the configuration
        /// </para>
        /// </summary>
        /// <value>
        /// Minimum node size (height).
        /// </value>
        public int MinimumNodeHeight
        {
            get
            {
                return Helper.ReadConfigInt(config, MINIMUM_NODE_HEIGHT);
            }
        }

        /// <summary>
        /// <para>
        /// Getter which returns the currently stored value under
        /// the 'minimum_synthetic_node_width' in the configuration.
        /// </para>
        /// </summary>
        /// <value>
        /// Minimum synthetic node size. (width).
        /// </value>
        public int MinimumSyntheticNodeWidth
        {
            get
            {
                return Helper.ReadConfigInt(config, MINIMUM_SYNTHETIC_NODE_WIDTH);
            }
        }

        /// <summary>
        /// <para>
        /// Getter which returns the currently stored value under
        /// the 'minimum_synthetic_node_height' in the configuration.
        /// </para>
        /// </summary>
        /// <value>
        /// Minimum synthetic node size. (height).
        /// </value>
        public int MinimumSyntheticNodeHeight
        {
            get
            {
                return Helper.ReadConfigInt(config, MINIMUM_SYNTHETIC_NODE_HEIGHT);
            }
        }

        /// <summary>
        /// <para>Getter which returns the currently stored value
        /// under the 'minimum_unlinked_port_space' in the configuration</para>
        /// </summary>
        public int MinimumUnlinkedPortSpace
        {
            get
            {
                return Helper.ReadConfigInt(config, MINIMUM_UNLINKED_PORT_SPACE);
            }
        }

        /// <summary>
        /// <para>
        /// Getter which gets the graph layouter used by this layout engine.
        /// </para>
        /// </summary>
        /// <value>The graph layouter used by this layout engine.</value>
        public IGraphLayouter GraphLayouter
        {
            get
            {
                return graphLayouter;
            }
        }

        /// <summary>
        /// <para>Gets the Logger used by this layout engine.</para>
        /// </summary>
        /// <value>The Logger used by this layout engine.</value>
        public Logger Logger
        {
            get
            {
                return logger;
            }
        }

        /// <summary>
        /// <para>Getter which returns IConfiguration object used by this layout engine.</para>
        /// </summary>
        /// <value>The IConfiguration object used by this layout engine.</value>
        protected IConfiguration Config
        {
            get
            {
                return config;
            }
        }

        /// <summary>
        /// <para>Creates a new BaseLayoutEngine instance using the data from configuration.</para>
        /// </summary>
        /// <param name="config">configuration object</param>
        /// <exception cref="SelfDocumentingException">
        /// Wraps ArgumentNullException if config is null.
        /// Wraps ConfigurationAPIException if any required element in config is missing or has invalid value.
        /// </exception>
        protected BaseLayoutEngine(IConfiguration config)
        {
            string loggerNs = null;
            string layouterToken = null;
            string ofNs = null;
            try
            {
                Helper.ValidateNotNull(config, "config");

                //Create logger if necessary
                loggerNs = Helper.ReadConfig(config, LOGGER_NAME, false);
                if (loggerNs != null)
                {      
                    logger = LogManager.CreateLogger(loggerNs);
                }

                //Create the IGraphLayouter instance
                layouterToken = Helper.ReadConfig(config, GRAPH_LAYOUTER_TOKEN, true);
                ofNs = Helper.ReadConfig(config, OBJECT_FACTORY_NS, false);
                ObjectFactory of = ofNs == null ?
                    ObjectFactory.GetDefaultObjectFactory() :
                    ObjectFactory.GetDefaultObjectFactory(ofNs);

                graphLayouter = (IGraphLayouter)of.CreateDefinedObject(layouterToken);

                //Check config properties. We just check for format and do not use the return value.
                Helper.ReadConfigDouble(config, DEFAULT_FONT_SIZE);
                Helper.ReadConfigDouble(config, FONT_UNITS);
                Helper.ReadConfigDouble(config, CHARACTER_UNITS);
                Helper.ReadConfigInt(config, MINIMUM_PORT_WIDTH);
                Helper.ReadConfigInt(config, MINIMUM_PORT_HEIGHT);
                Helper.ReadConfigInt(config, MINIMUM_LINK_SPACE);
                Helper.ReadConfigInt(config, MINIMUM_NODE_WIDTH);
                Helper.ReadConfigInt(config, MINIMUM_NODE_HEIGHT);
                Helper.ReadConfigInt(config, MINIMUM_SYNTHETIC_NODE_WIDTH);
                Helper.ReadConfigInt(config, MINIMUM_SYNTHETIC_NODE_HEIGHT);
                Helper.ReadConfigInt(config, MINIMUM_UNLINKED_PORT_SPACE);

                //Clone the config and store it
                this.config = config.Clone() as IConfiguration;
            }
            catch (Exception e)
            {
                throw Helper.GetSelfDocumentingException(e, "Unable to create BaseLayoutEngine instance.",
                    "Astraea.Inframap.Layout.BaseLayoutEngine.BaseLayoutEngine",
                    new string[] { "config", "graphLayouter", "logger" },
                    new object[] { config, graphLayouter, logger },
                    new string[] { "config" }, new object[] { config },
                    new string[] { "loggerNs", "layouterToken", "ofNs" },
                    new object[] { loggerNs, layouterToken, ofNs });
            }
        }

        /// <summary>
        /// <para>This is a contract for pre-processing of map data.</para>
        /// </summary>
        /// <param name="mapdata">mapdata to be pre-processed</param>
        /// <returns>the pre-processed mapdata</returns>
        /// <exception cref="SelfDocumentingException">
        /// Wraps ArgumentNullException if mapdata is null.
        /// </exception>
        /// <exception cref="LayoutPreProcessingException">
        /// If there are any exceptions in the pre-processing of the map data.
        /// </exception>
        protected abstract MapData PreProcess(MapData mapdata);

        /// <summary>
        /// <para>This is a contract for post-processing of map data.
        /// This method is called after the ProcessLayouting method is called.</para>
        /// </summary>
        /// <param name="mapdata">mapdata to be post-processed</param>
        /// <returns>the post-processed mapdata</returns>
        /// <exception cref="SelfDocumentingException">
        /// Wraps ArgumentNullException if mapdata is null.
        /// </exception>
        /// <exception cref="LayoutPostProcessingException">
        /// If there are any exceptions in the post-processing of the map data.
        /// </exception>
        protected abstract MapData PostProcess(MapData mapdata);

        /// <summary>
        /// <para>This is a concrete method which simply delegates to the graphLayouter.</para>
        /// </summary>
        /// <param name="mapdata">mapdata to be processed with layouting</param>
        /// <returns>mapdata that has been processed with layouting</returns>
        /// <exception cref="SelfDocumentingException">
        /// Wraps ArgumentNullException if mapdata is null.
        /// </exception>
        /// <exception cref="LayoutException">
        /// If there are any exceptions in the laying out of the map data.
        /// </exception>
        protected virtual MapData PerformLayout(MapData mapdata)
        {
            Helper.Log(logger, Level.INFO, "Entering PerformLayout with mapdata {0}.", new object[] { mapdata });

            MapData result = null;
            try
            {
                Helper.ValidateNotNull(mapdata, "mapdata");

                //Delegate to the GraphLayouter's Layout method
                result = (MapData)(graphLayouter.Layout(mapdata));

                Helper.Log(logger, Level.INFO, "Exiting PerformLayout with return value {0}.", new object[] { result });
                return result;
            }
            catch (Exception e)
            {
                Helper.Log(logger, Level.ERROR,
                    "Error in PerformLayout with error message {0}.", new object[] { e.Message });

                throw Helper.GetSelfDocumentingException(e, "Unable to perform layout.",
                    "Astraea.Inframap.Layout.BaseLayoutEngine.PerformLayout",
                    new string[] { "config", "graphLayouter", "logger" },
                    new object[] { config, graphLayouter, logger },
                    new string[] { "mapdata" }, new object[] { mapdata },
                    new string[] { "result" }, new object[] { result });
            }
            finally
            {
                Helper.Log(logger, Level.INFO, "Exiting PerformLayout with mapdata {0}.", new object[] { mapdata });
            }
        }

        /// <summary>
        /// <para>This method generates a unique id (always positive but unique amongst both
        /// negative and positive ids) for the specific type and within the specific mapdata context.</para>
        /// <para>Currently the following types are distinctly supported:</para>
        /// <li>Astraea.Inframap.Layout.Data.MapNode</li>
        /// <li>Astraea.Inframap.Layout.Data.MapLink</li>
        /// <li>Astraea.Inframap.Layout.Data.Map.Port</li>
        /// </summary>
        /// <param name="entityType">this is the type of entity that we generate the id for.</param>
        /// <param name="mapdata">this is the context for which the id must be unique</param>
        /// <returns>the generated unique id</returns>
        /// <exception cref="SelfDocumentingException">
        /// Wraps ArgumentNullException if entityType or mapdata is null.
        /// Wraps ArgumentException if entityType is not a supported type.
        /// </exception>
        protected virtual long GenerateUniqueId(Type entityType, MapData mapdata)
        {
            Helper.Log(logger, Level.INFO,
                "Entering GenerateUniqueId with entityType: {0} mapdata {1}.",
                new object[] { entityType, mapdata });

            long maxId = 0;
            try
            {
                //Validate
                Helper.ValidateNotNull(entityType, "entityType");
                Helper.ValidateNotNull(mapdata, "mapdata");
                if (entityType != typeof(MapNode) &&
                    entityType != typeof(MapLink) &&
                    entityType != typeof(MapPort) &&
                    entityType != typeof(MapAttribute))
                {
                    throw new ArgumentException(
                        "Unique ids are supported only for types MapNode, MapLink and MapPort.", "entityType");
                }

                //Get the appropriate list from the MapData
                IEnumerator typeListEn = null;
                if (entityType.Equals(typeof(MapNode)))
                {
                    typeListEn = mapdata.Nodes.GetEnumerator();
                }
                else if (entityType.Equals(typeof(MapLink)))
                {
                    typeListEn = mapdata.Links.GetEnumerator();
                }
                else if (entityType.Equals(typeof(MapPort)))
                {
                    typeListEn = mapdata.Ports.GetEnumerator();
                }
                else if (entityType.Equals(typeof(MapAttribute)))
                {
                    typeListEn = mapdata.Attributes.GetEnumerator();
                }

                if (entityType.Equals(typeof(MapAttribute)))
                {
                    //Get the maximum id of the attributes in the list.
                    while (typeListEn.MoveNext())
                    {
                        MapAttribute mapAttribute = (MapAttribute)typeListEn.Current;
                        maxId = Math.Max(maxId, Math.Abs(mapAttribute.Id));
                    }
                }
                else
                {
                    //Get the maximum id of the elements in the list.
                    while (typeListEn.MoveNext())
                    {
                        MapElement mapElement = (MapElement)typeListEn.Current;
                        maxId = Math.Max(maxId, Math.Abs(mapElement.Id));
                    }
                }

                return maxId + 1;
            }
            catch (Exception e)
            {
                Helper.Log(logger, Level.ERROR,
                    "Error in GenerateUniqueId with error: {0}.", new object[] { e.Message });

                throw Helper.GetSelfDocumentingException(e, "Unable to generate unique id.",
                    "Astraea.Inframap.Layout.BaseLayoutEngine.GenerateUniqueId",
                    new string[] { "config", "graphLayouter", "logger" },
                    new object[] { config, graphLayouter, logger },
                    new string[] { "entityType", "mapdata" }, new object[] { entityType, mapdata },
                    new string[0], new object[0]);
            }
            finally
            {
                Helper.Log(logger, Level.INFO,
                     "Exiting GenerateUniqueId with return value: {0}.", new object[] { maxId + 1 });
            }
        }

        /// <summary>
        /// <para>This is a concrete method which simply coordinates the pre-processing
        /// the processing and the post-processing of a MapData instance for a layouting request.</para>
        /// </summary>
        /// <param name="mapdata">mapdata to be fully processed with layouting</param>
        /// <returns>the fully layouted mapdata</returns>
        /// <exception cref="SelfDocumentingException">
        /// Wraps ArgumentNullException if mapdata is null.
        /// </exception>
        /// <exception cref="LayoutException">
        /// If there are any exceptions in the laying out of the map data.
        /// </exception>
        /// <exception cref="LayoutPreProcessingException">
        /// If there are any exceptions in the pre-processing of the map data.
        /// </exception>
        /// <exception cref="LayoutPostProcessingException">
        /// If there are any exceptions in the post-processing of the map data.
        /// </exception>
        public MapData Layout(MapData mapdata)
        {
            Helper.Log(logger, Level.INFO, "Entering Layout with mapdata {0}.", new object[] { mapdata });

            MapData result = null;
            try
            {
                Helper.ValidateNotNull(mapdata, "mapdata");

                //Delagate to PreProcess, PerformLayout, PostProcess
                result = PreProcess(mapdata);
                result = PerformLayout(result);
                result = PostProcess(result);

                //Set MapHeader lifecycle
                result.Header.Lifecycle = LIFECYCLE_LAYOUT;

                return result;
            }
            catch (Exception e)
            {
                Helper.Log(logger, Level.ERROR,
                    "Error in Layout with error: {0}.", new object[] { e.Message });

                throw Helper.GetSelfDocumentingException(e, "Unable to layout the mapdata.",
                    "Astraea.Inframap.Layout.BaseLayoutEngine.Layout",
                    new string[] { "config", "graphLayouter", "logger" },
                    new object[] { config, graphLayouter, logger },
                    new string[] { "mapdata" }, new object[] { mapdata },
                    new string[] { "result" }, new object[] { result });

            }
            finally
            {
                Helper.Log(logger, Level.INFO, "Exiting Layout with mapdata {0}.", new object[] { mapdata });
            }
        }


    }
}
