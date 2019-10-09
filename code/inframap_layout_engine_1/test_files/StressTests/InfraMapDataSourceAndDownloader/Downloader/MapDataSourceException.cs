
package Astraea.Inframap.Downloader;
import Astraea.Inframap.Data.*;
import System.Runtime.Serialization.SerializationInfo;
import System.Runtime.Serialization.StreamingContext;
import System.string;

/**
 * This exception is thrown by all methods in the downloader and retriever indicating there was an error during a retrieval operation. It extends SelfDocumentingException.
 * 
 */
public class MapDataSourceException :SelfDocumentingException {

/**
 * <p>Initializes a new instance of this class with its message string set to a system-supplied message.</p>
 * 
 * 
 */
    public  MapDataSourceException() {        
        // your code here
    } 

/**
 * <p>Initializes a new instance of this class with a specified error message.</p>
 * 
 * 
 * 
 * @param message A string message that describes the error.
 */
    public  MapDataSourceException(string message) {        
        // your code here
    } 

/**
 * <p>Initializes a new instance of this class with a specified error message and a reference to the inner exception that is the cause of this exception.</p>
 * 
 * 
 * 
 * @param message A string message that describes the error.
 * @param cause The exception that is the cause of the current exception.
 */
    public  MapDataSourceException(string message, Exception cause) {        
        // your code here
    } 

/**
 * <p>Initializes a new instance of this class with the specified serialization and context information.</p>
 * 
 * 
 * 
 * @param info The object that holds the serialized object data.
 * @param context The contextual information about the source or destination.
 */
    protected  MapDataSourceException(SerializationInfo info, StreamingContext context) {        
        // your code here
    } 
 }
