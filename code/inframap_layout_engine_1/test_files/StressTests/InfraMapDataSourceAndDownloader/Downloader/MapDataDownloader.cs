using TopCoder.Configuration;
using TopCoder.Cache;
using Astraea.Inframap.Data;
using System.Collections.Generic;
namespace Astraea.Inframap.Downloader
{

    ///  <summary>
    ///  <p>The main manager used to retrieve map data, styles, element types, and attribute types. It relies on the MapDataRetriever to perform the actual data retrieval. It adds the ability to cache the data using SimpleCaches.</p>
    ///  <p>Thread Safety: This class is mutable but thread-safe. The mutability comes from the changing state of the caches. Otherwise, the class is not externally mutable.</p>         ///  </summary>
    public class MapDataDownloader
    {

        /// Attribute retriever
        /// <summary>
        /// Represents the data store retriever of the map data. It will be used in all download operations. If the caches are functional, then the retriever will be used only if the caches have expired. This is obtained in the constructor, and will never change or be null.           /// </summary>
        private readonly IMapDataRetriever retriever;

        /// Attribute mapDataCache
        /// <summary>
        /// Represents the simple cache that will contain MapData instances that are mapped to mapId long keys. If functional, it will be used in the DownloadMapData method. This is obtained in the constructor, and may be null to indicate no caching is done for MapData.           /// </summary>
        private readonly SimpleCache mapDataCache;

        /// Attribute styleCache
        /// <summary>
        /// Represents the simple cache that will contain a IList of MapStyle instance with mapId long keys. If functional, it will be used in the DownloadStyles method. This is obtained in the constructor, and may be null to indicate no caching is done for MapStyles.           /// </summary>
        private readonly SimpleCache styleCache;

        /// Attribute attributeTypeCache
        /// <summary>
        /// Represents the simple cache that will contain a IList of MapAttributeType instance with the key &quot;atributeTypes&quot;. If functional, it will be used in the DownloadAttributeTypes method. This is obtained in the constructor, and may be null to indicate no caching is done for MapAttributeTypes.           /// </summary>
        private readonly SimpleCache attributeTypeCache;

        /// Attribute elementTypeCache
        /// <summary>
        /// Represents the simple cache that will contain a IList of MapElementType instance with the key &quot;elementTypes&quot;. If functional, it will be used in the DownloadElementTypes method. This is obtained in the constructor, and may be null to indicate no caching is done for MapElementTypes.           /// </summary>
        private readonly SimpleCache elementTypeCache;

        /// <summary>
        /// Constructor: Obtains the keys for the simple caches and map data retriever from The IConfiguration, and then uses the ConfigurationAPIObjectFactory to obtain instances of these.           /// </summary>
        /// <exception>ArgumentNullException If configuration is null</exception>
        /// <exception>ArgumentException If configuration is missing required parameters</exception>
        /// <param name='configuration'>The configuration object with the keys for the refreshable caches and map data retriever</param>
        public MapDataDownloader(IConfiguration configuration)
        {
            Helper.CheckNull(configuration, "configuration");
        }

        /// Constructor MapDataDownloader
        /// <summary>
        /// Constructor: Simply sets the passed parameters to the namesake fields.
        /// </summary>
        /// <exception>ArgumentNullException If retriever is null</exception>
        /// <param name='retriever'>the data store retriever of the map data</param>
        /// <param name='mapDataCache'>the simple cache that will cache map data</param>
        /// <param name='styleCache'>the simple cache that will cache styles</param>
        /// <param name='attributeTypeCache'>the simple cache that will cache attribute types</param>
        /// <param name='elementTypeCache'>the simple cache that will cache element types</param>
        public MapDataDownloader(IMapDataRetriever retriever, SimpleCache mapDataCache, SimpleCache styleCache, SimpleCache attributeTypeCache, SimpleCache elementTypeCache)
        {
            Helper.CheckNull(retriever, "retriever");

            this.retriever = retriever;
            this.mapDataCache = mapDataCache;
            this.styleCache = styleCache;
            this.attributeTypeCache = attributeTypeCache;
            this.elementTypeCache = elementTypeCache;
        }

        /// Operation DownloadMapData
        /// <summary>
        /// Gets the map data with the given ID.
        /// <p><strong>Implementation Notes</strong></p>
        /// <ul type="disc">
        /// <li>If mapDataCache is not null, and if mapDataCache has an entry for this mapId, return the MapData for that ID.</li>
        /// <li>Call retriever.DownloadMapData(mapId) to get the MapData.</li>
        /// <li>If mapDataCache is not null, add the retrieved MapData mapDataCache with the mapId as the key.</li>
        /// </ul>           /// </summary>
        /// <exception>MapIdNotFoundException If there is no MapData with the given ID</exception>
        /// <exception>MapDataSourceException If there were errors during this operation</exception>
        /// <param name='mapId'>The ID of the MapData to be retrieved</param>
        /// <returns>Retrieved MapData</returns>
        public MapData DownloadMapData(long mapId)
        {
            if ((mapDataCache != null) && mapDataCache.ContainsKey(mapId))
            {
                return (MapData)mapDataCache[mapId];
            }
            MapData mapData = retriever.DownloadMapData(mapId);

            if (mapDataCache != null)
            {
                mapDataCache.Add(mapId, mapData);
            }
            return mapData;
        }

        /// Operation DownloadStyles
        /// <summary>
        /// Gets all styles of the map data with the given ID. Returns an empty list if none found. It will never return null or a list with null elements.
        /// <p><strong>Implementation Notes</strong></p>
        /// <ul type="disc">
        /// <li>If styleCache is not null, and if styleCache has an entry for this mapId, return the IList of MapStyle for that ID.</li>
        /// <li>Call retriever.DownloadStyles(mapId) to get the IList of MapStyle.</li>
        /// <li>If styleCache is not null, add the retrieved IList of MapStyle to styleCache with the mapId as the key.</li>
        /// </ul>           /// </summary>
        /// <exception>MapIdNotFoundException If there is no MapData with the given ID</exception>
        /// <exception>MapDataSourceException If there were errors during this operation</exception>
        /// <param name='mapId'>The ID of the MapData whose styles are to be retrieved</param>
        /// <returns>List of MapStyles of the map data with the given ID</returns>
        public IList<MapStyle> DownloadStyles(long mapId)
        {
            if ((styleCache != null) && styleCache.ContainsKey(mapId))
            {
                return (IList<MapStyle>) styleCache[mapId];
            }
            IList<MapStyle> styles = retriever.DownloadStyles(mapId);

            if (styleCache != null)
            {
                styleCache.Add(mapId, styles);
            }

            return styles;
        }

        /// Operation DownloadAttributeTypes
        /// <summary>
        /// Gets all available MapAttributeTypes. Returns an empty list if none found. It will never return null or a list with null elements.
        /// <p><strong>Implementation Notes</strong></p>
        /// <ul type="disc">
        /// <li>If attributeTypeCache is not null, and if attributeTypeCache has an entry for &quot;attributeTypes&quot;, return the IList of MapAttributeType for that key.</li>
        /// <li>Call retriever.DownloadAttributeTypes() to get the IList of MapAttributeType.</li>
        /// <li>If attributeTypeCache is not null, add the retrieved IList of MapAttributeType to attributeTypeCache with &quot;attributeTypes&quot; as the key.</li>
        /// </ul>           /// </summary>
        /// <exception>MapDataSourceException If there were errors during this operation</exception>
        /// <returns>List of available MapAttributeTypes</returns>
        public IList<MapAttributeType> DownloadAttributeTypes()
        {
            if ((attributeTypeCache != null) && attributeTypeCache.ContainsKey("attributeTypes"))
            {
                return (IList<MapAttributeType>) attributeTypeCache["attributeTypes"];
            }
            IList<MapAttributeType> attributeTypes = retriever.DownloadAttributeTypes();

            if (attributeTypeCache != null)
            {
                attributeTypeCache.Add("attributeTypes", attributeTypes);
            }

            return attributeTypes;

        }

        /// Operation DownloadElementTypes
        /// <summary>
        /// Gets all available MapElementTypes. Returns an empty list if none found. It will never return null or a list with null elements.
        /// <p><strong>Implementation Notes</strong></p>
        /// <ul type="disc">
        /// <li>If elementTypeCache is not null, and if elementTypeCache has an entry for &quot;elementTypes&quot;, return the IList of MapElementType for that key.</li>
        /// <li>Call retriever.DownloadElementTypes() to get the IList of MapElementType.</li>
        /// <li>If elementTypeCache is not null, add the retrieved IList of MapElementType to elementTypeCache with &quot;elementTypes&quot; as the key.</li>
        /// </ul>           /// </summary>
        /// <exception>MapDataSourceException If there were errors during this operation</exception>
        /// <returns>List of available MapElementTypes</returns>
        public IList<MapElementType> DownloadElementTypes()
        {
            if ((elementTypeCache != null) && elementTypeCache.ContainsKey("elementTypes"))
            {
                return (IList<MapElementType>) elementTypeCache["elementTypes"];
            }
            IList<MapElementType> elementTypes = retriever.DownloadElementTypes();

            if (elementTypeCache != null)
            {
                elementTypeCache.Add("elementTypes", elementTypes);
            }

            return elementTypes;
        }
    }
}