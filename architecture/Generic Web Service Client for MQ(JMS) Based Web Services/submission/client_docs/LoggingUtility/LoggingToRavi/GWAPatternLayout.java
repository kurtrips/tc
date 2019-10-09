


package financing.tools.docgen.util;

import org.apache.log4j.PatternLayout;
import org.apache.log4j.helpers.PatternParser;

/**
 * The GWAPatternLayout class extends Log4j's PatternLayout.  The
 * createPatternParser method is the only method that required overwriting,
 * since a GWAPatternParser class needs to be called instead of the standard
 * Log4j class PatternParser.
 *
 * @author	Nisha
 *
 * @see org.apache.log4j.PatternLayout
 */
public class GWAPatternLayout extends PatternLayout {
	
	/**
	 * Creates a new GWAPatternLayout object.
	 */
	public GWAPatternLayout() {
		this(DEFAULT_CONVERSION_PATTERN);
	}

	/**
	 * Creates a new GWAPatternLayout object.
	 *
	 * @param pattern parameter
	 */
	public GWAPatternLayout(String pattern) {
		super(pattern);
	}

	/**
	 * The createPatternParser method creates a new instance of the
	 * GWAPatternParser with the  pattern String that was passed into the
	 * method (uses Log4j default if pattern is null).
	 *
	 * @param pattern parameter
	 * @return return the GWAPatternParser
	 */
	public PatternParser createPatternParser(String pattern) {
		PatternParser result;

		if (pattern == null) {
			result = new GWAPatternParser(DEFAULT_CONVERSION_PATTERN);
		} else {
			result = new GWAPatternParser(pattern);
		}

		return result;
	}
	
}
