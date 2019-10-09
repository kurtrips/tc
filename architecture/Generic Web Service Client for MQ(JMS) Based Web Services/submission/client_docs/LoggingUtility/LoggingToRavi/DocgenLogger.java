
package financing.tools.docgen.util;

import org.apache.log4j.LogManager;
import org.apache.log4j.Logger;

/**
 * This is a log4j wrapper class for Docgen. It is used to output statements to the Docgen log files.
 * 
 * @author Ravi
 * @version $Revision: 1.0 $
 */
public class DocgenLogger {
    /** Root logger */
    private static Logger rootLogger = null;

    /**
     * GcmsLogger constructor.
     */
    private DocgenLogger() {
	super();
	rootLogger = LogManager.getRootLogger();
    }

    /**
     * Initialization method. Initialize the log4j properties file path and file name.
     * 
     */
    private static void init() {
	/*
	 * String filePath=ClassLoader.getSystemResource(DGSProperties.getInstance().getValueFromProperties(DocgenConstants.DGS_LOG4J_PROPERTIES)).toString(); try{ PropertyConfigurator.configure(new URL(filePath)); }catch(MalformedURLException x){
	 * fatal(DocgenLogger.class.getName(),x.getMessage()); }
	 */
    }

    /**
     * Get logger object.
     * 
     * @param logClass
     *            String
     * @return Logger
     */
    protected static Logger getLog4jLogger(String logClass) {
	Logger logger = null;

	if (null == logClass) {
	    return getRootLogger();
	}
	logger = LogManager.getLogger(logClass);

	if (null == logger) {
	    logger = getRootLogger();
	}

	return logger;
    }

    /**
     * Get root logger.
     * 
     * @return Logger
     */
    protected static Logger getRootLogger() {
	if (rootLogger == null) {
	    rootLogger = LogManager.getRootLogger();
	}

	return rootLogger;
    }

    /**
     * Output debug statement to log file.
     * 
     * @param logClass
     *            String - logger class invoking the logger
     * @param msg
     *            String - message
     */
    public static void debug(String logClass, String msg) {
	init();
	Logger logger = getLog4jLogger(logClass);
	logger.debug(msg);
    }

    /**
     * Output error statement to log file.
     * 
     * @param logClass
     *            String - logger class invoking the logger
     * @param msg
     *            String - message
     */
    public static void error(String logClass, String msg) {
	init();
	Logger logger = getLog4jLogger(logClass);
	logger.error(msg);
    }

    /**
     * Output fatal statement to log file.
     * 
     * @param logClass
     *            String - logger class invoking the logger
     * @param msg
     *            String - message
     */
    public static void fatal(String logClass, String msg) {
	init();
	Logger logger = getLog4jLogger(logClass);
	logger.fatal(msg);
    }

    /**
     * Output info statement to log file.
     * 
     * @param logClass
     *            String - logger class invoking the logger
     * @param msg
     *            String - message
     */
    public static void info(String logClass, String msg) {
	init();
	Logger logger = getLog4jLogger(logClass);
	logger.info(msg);
    }

    /**
     * Output warning statement to log file.
     * 
     * @param logClass
     *            String - logger class invoking the logger
     * @param msg
     *            String - message
     */
    public static void warn(String logClass, String msg) {
	init();
	Logger logger = getLog4jLogger(logClass);
	logger.warn(msg);
    }

    public static void fatal(String logClass, Exception e) {
	init();
	Logger logger = getLog4jLogger(logClass);
	logger.fatal(logClass, e);
    }

}
