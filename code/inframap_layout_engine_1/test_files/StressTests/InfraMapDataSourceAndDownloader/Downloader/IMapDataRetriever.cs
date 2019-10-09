using System.Collections.Generic;
using Astraea.Inframap.Data;

namespace Astraea.Inframap.Downloader
{

    ///  <summary>
    ///  <p>The interface specifying the contract for retrieving map data, styles, element types, and attribute types. It has one concrete implementation: SqlServerMapDataRetriever.</p>
    ///  <p>Thread Safety: There are no specific restrictions on thread-safety.</p>         ///  </summary>
    public interface IMapDataRetriever
    {

        /// <summary>
        /// Gets the map data with the given ID.
        /// </summary>
        /// <exception>MapIdNotFoundException If there is no MapData with the given ID</exception>
        /// <exception>MapDataSourceException If there were errors during this operation</exception>
        /// <param name='mapId'>The ID of the MapData to be retrieved</param>
        /// <returns>Retrieved MapData</returns>
        MapData DownloadMapData(long mapId);

        /// Operation DownloadStyles
        /// <summary>
        /// Gets all styles of the map data with the given ID. Returns an empty list if none found. It will never return null or a list with null elements.           /// </summary>
        /// <exception>MapIdNotFoundException If there is no MapData with the given ID</exception>
        /// <exception>MapDataSourceException If there were errors during this operation</exception>
        /// <param name='mapId'>The ID of the MapData whose styles are to be retrieved</param>
        /// <returns>List of MapStyles of the map data with the given ID</returns>
        IList<MapStyle> DownloadStyles(long mapId);

        /// Operation DownloadAttributeTypes
        /// <summary>
        /// Gets all available MapAttributeTypes. Returns an empty list if none found.
        /// It will never return null or a list with null elements.
        /// </summary>
        /// <exception>MapDataSourceException If there were errors during this operation</exception>
        /// <returns>List of available MapAttributeTypes</returns>
        IList<MapAttributeType> DownloadAttributeTypes();

        /// Operation DownloadElementTypes
        /// <summary>
        /// Gets all available MapElementTypes. Returns an empty list if none found. It will never return null or a list with null elements.           /// </summary>
        /// <exception>MapDataSourceException If there were errors during this operation</exception>
        /// <returns>List of available MapElementTypes</returns>
        IList<MapElementType> DownloadElementTypes();
    }
}