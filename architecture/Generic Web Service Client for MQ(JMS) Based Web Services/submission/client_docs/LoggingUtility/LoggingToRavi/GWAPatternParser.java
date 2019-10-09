


package financing.tools.docgen.util;

import org.apache.log4j.Level;
import org.apache.log4j.helpers.FormattingInfo;
import org.apache.log4j.helpers.PatternConverter;
import org.apache.log4j.helpers.PatternParser;
import org.apache.log4j.spi.LoggingEvent;

/**
 * The GWAPatternParser class extends the Log4j PatternParser. A sinlge
 * conversion character 's' is added.  This will mark the severity of the log
 * event.  The severity is determined by the level of the log event and will
 * be mapped as follows: DEBUG (4),  INFO (4), WARN (3), ERROR (2), and FATAL
 * (1).
 *
 * @author	teddyng
 *
 * @see org.apache.log4j.helpers.PatternParser
 */
public class GWAPatternParser extends PatternParser {
	private static final char SEVERITY_CHAR = 's';
	private static final String DEBUG_SEVERITY_VALUE = "4";
	private static final String INFO_SEVERITY_VALUE = "4";
	private static final String WARN_SEVERITY_VALUE = "3";
	private static final String ERROR_SEVERITY_VALUE = "2";
	private static final String FATAL_SEVERITY_VALUE = "1";
	private static final String UNKNOWN_SEVERITY_VALUE = "-1";

	/**
	 * Creates a new GWAPatternParser object.
	 *
	 * @param pattern parameter
	 */
	public GWAPatternParser(String pattern) {
		super(pattern);
	}

	/**
	 * The finalizeConverter method will check the conversion character passed
	 * into the method to see if it matches the severity character ('s'). If
	 * so, the SeverityPatternConverter  internal class will be instantiated.
	 * If it is any other character, the super class's finalizeConverter
	 * method will be called.
	 *
	 * @param formatChar parameter
	 */
	public void finalizeConverter(char formatChar) {
		PatternConverter pc = null;

		if (formatChar == SEVERITY_CHAR) {
			pc = new SeverityPatternConverter(formattingInfo);
			currentLiteral.setLength(0);
			addConverter(pc);
		} else {
			super.finalizeConverter(formatChar);
		}
	}

	private static class SeverityPatternConverter extends PatternConverter {
		SeverityPatternConverter(FormattingInfo formattingInfo) {
			super(formattingInfo);
		}

		/**
		 * The convert method will determine the appropriate severity result
		 * for the LoggingEvent. The level is obtained from the event.  The
		 * severitys are mapped to the level as follows: DEBUG (4), INFO (4),
		 * WARN (3), ERROR (2), and FATAL (1).  If there is a level that is
		 * unknown, severity is defaulted to -1.
		 *
		 * @param event parameter
		 *
		 * @return return
		 */
		public String convert(LoggingEvent event) {
			String severity = null;
			Level level = event.getLevel();
			int levelIntValue = level.toInt();

			if (levelIntValue == Level.DEBUG_INT) {
				severity = DEBUG_SEVERITY_VALUE;
			} else if (levelIntValue == Level.INFO_INT) {
				severity = INFO_SEVERITY_VALUE;
			} else if (levelIntValue == Level.WARN_INT) {
				severity = WARN_SEVERITY_VALUE;
			} else if (levelIntValue == Level.ERROR_INT) {
				severity = ERROR_SEVERITY_VALUE;
			} else if (levelIntValue == Level.FATAL_INT) {
				severity = FATAL_SEVERITY_VALUE;
			} else {
				severity = UNKNOWN_SEVERITY_VALUE;
			}

			return severity;
		}
	}
}
