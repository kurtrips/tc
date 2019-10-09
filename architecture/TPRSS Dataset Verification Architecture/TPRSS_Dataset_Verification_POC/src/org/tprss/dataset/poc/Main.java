package org.tprss.dataset.poc;

import java.io.BufferedWriter;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.FileWriter;
import java.io.IOException;
import java.io.OutputStream;
import java.nio.charset.Charset;
import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import org.apache.pdfbox.cos.*;
import org.apache.pdfbox.pdfparser.*;
import org.apache.pdfbox.pdfwriter.*;
import org.apache.pdfbox.pdmodel.*;
import org.apache.pdfbox.pdmodel.common.*;
import org.apache.pdfbox.util.*;
import org.apache.poi.hwpf.HWPFDocument;
import org.apache.poi.hwpf.usermodel.*;
import org.apache.poi.poifs.filesystem.POIFSFileSystem;

public class Main {

	static String inputPdfFile1 = "./input/86BSS1.pdf";
	static String inputPdfFile2 = "./input/068875.pdf";
	static String inputPdfFile3 = "./input/150411.pdf";
	static String inputPdfFile4 = "./input/302489.pdf";
	static String inputPdfFile5 = "./input/861221.pdf";
	static String inputWordFile = "./input/TPRSS Asset Protection Plan V2 06-2012.doc";
	
	static final char BEL = 0x07;
	static final String USERS_TABLE_HEADER = "UseridNameOwnerIdent.St.Sec.byCondition";
	static final String GROUPS_TABLE_HEADER = "OwnerIdent.byConditionconnecteduserssee";
	
	static final Map<String, List<AccessInfo>> pdfDataSets = new HashMap<String, List<AccessInfo>>();
	
	static BufferedWriter pdfOut;
	static BufferedWriter wordOut;
	
	/**
	 * @param args
	 * @throws IOException 
	 */
	public static void main(String[] args) throws IOException {
		pdfOut = new BufferedWriter(new FileWriter("./output/Pdf.txt"));
		wordOut = new BufferedWriter(new FileWriter("./output/Word.txt"));

		doThePdfThing(inputPdfFile1);
		doThePdfThing(inputPdfFile2);
		doThePdfThing(inputPdfFile3);
		doThePdfThing(inputPdfFile4);
		doThePdfThing(inputPdfFile5);
		doTheWordThing();
		
		pdfOut.flush();
		wordOut.flush();
		pdfOut.close();
		wordOut.close();
		
		System.out.println("Done!");
	}

	static void doThePdfThing(String fileName) throws IOException {
        // the document
        PDDocument doc = null;
        try
        {
            String currentDataSet = null;
            boolean datasetStart = false;
            boolean userDataStart = false;
            boolean groupDataStart = false;
            boolean userIdWithAccessLineFound = false;
            boolean groupsWithAccessLineFound = false;
            String currentAccessValue = null;
            int groupRead = 0;
            String currentFontStyle = null;
            float currentFontSize = 0;
            
        	doc = PDDocument.load( fileName );
            List pages = doc.getDocumentCatalog().getAllPages();
            for( int i=0; i<pages.size(); i++ )
            {
                //Parse through the page
            	PDPage page = (PDPage)pages.get( i );
                PDStream contents = page.getContents();
                PDFStreamParser parser = new PDFStreamParser(contents.getStream());
                parser.parse();
                
                //Go through each PDF token
                List tokens = parser.getTokens();
                for( int j=0; j<tokens.size(); j++ ) {
	                
                	Object next = tokens.get( j );
	                if(!(next instanceof PDFOperator))
	                	continue;
                    PDFOperator op = (PDFOperator)next;
                    //System.out.println(op.getOperation());
                    
                    //This is used to store the font that is being used to write the proceeding text
                    if( op.getOperation().equals( "Tf" )) {
                    	COSFloat prev1 = (COSFloat)tokens.get( j-1 );
                    	COSName prev2 = (COSName)tokens.get( j-2 );
                    	currentFontStyle = prev2.getName();
                    	currentFontSize = prev1.floatValue();
                    }
                    
 
                    //TJ is used to display String in this PDF.
                    if( !(op.getOperation().equals( "TJ" )) )
                    	continue;

                    //Get the stuff before the TJ
                    //This is the text for this TJ operator
                    COSArray previous = (COSArray)tokens.get( j-1 );
                    String text = "";
                    for( int k=0; k<previous.size(); k++ ) {
                        Object arrElement = previous.getObject( k );
                        if( arrElement instanceof COSString ) {
                            COSString cosString = (COSString)arrElement;
                            text += cosString.getString();
                        }
                    }
                    
                    //Parse the line if it is dataset
                    if (text.startsWith("DATASET")) {
                    	currentDataSet = text.replace("DATASET", "");
                    	datasetStart = true;
                    	userDataStart = false;
                    	groupDataStart = false;
                    	//System.out.println(currentDataSet);
                    	continue;
                    }
                    
                    if (text.startsWith("ThereareNOexplicitaccessesspecified.") ||
                    		text.startsWith("GROUPSNOTOWNEDBY")) {
                    	//Even if the dataset is empty, we still need the key
                    	if (!pdfDataSets.containsKey(currentDataSet)) {
                    		pdfDataSets.put(currentDataSet, new ArrayList<AccessInfo>());
                    	}
                    	datasetStart = false;
                    	continue;
                    }
                    
                    //The text before the access level for users
                    if (text.equals("Userid(s)withaccess:")) {
                    	userIdWithAccessLineFound = true;
                    	userDataStart = false;
                    	groupDataStart = false;
                    	//System.out.println("Hi2");
                    	continue;
                    }
                    
                    //The text before the access level for groups
                    if (text.equals("GroupswithAccess:")) {
                    	groupsWithAccessLineFound = true;
                    	userDataStart = false;
                    	groupDataStart = false;
                    	//System.out.println("Hi3");
                    	continue;                    	
                    }
                    
                    //The actual access level text for users
                    if (userIdWithAccessLineFound){
                    	userIdWithAccessLineFound = false;
                    	currentAccessValue = text;
                    	//System.out.println("Hi4 " + text);
                    	continue;
                    }
                    
                    //The actual access level text for group
                    if (groupsWithAccessLineFound){
                    	groupsWithAccessLineFound = false;
                    	currentAccessValue = text;
                    	//System.out.println("Hi5 " + text);
                    	continue;
                    }                    

                    //We are the table header for the user access
                    if (text.equals(USERS_TABLE_HEADER)) {
                    	userDataStart = true;
                    	//System.out.println("Hi6");
                    	continue;
                    }

                    //We are the table header for the group access
                    if (text.equals(GROUPS_TABLE_HEADER)) {
                    	groupDataStart = true;
                    	//System.out.println("Hi7");
                    	continue;
                    }
                    
                    
                    if ((userDataStart || groupDataStart) && datasetStart) {
                    	String id = "";
                    	for( int k=0; k<previous.size(); k++ ) {
                    		Object arrElement = previous.getObject( k );
                            if( arrElement instanceof COSFloat ) {
                            	COSFloat cosFloat = (COSFloat)arrElement;
                            	//The value is large, so we know that the userId/groupId has been read
                            	if (Math.abs(cosFloat.floatValue()) > 500)
                            		break;
                            }
                            if( arrElement instanceof COSString ) {
                            	COSString cosString = (COSString)arrElement;
                            	id += cosString.getString();
                            }
                    	}
                    	
                    	//If this is not the font we expect for the tabular user/group data, then continue
                    	if (!currentFontStyle.equals("R21") || currentFontSize != 6.58776f) {
                        	userDataStart = false;
                        	groupDataStart = false;
                    		continue;
                    	}
                    	//Some corner cases
                    	if (!text.toUpperCase().equals(text)) {
                        	userDataStart = false;
                        	groupDataStart = false;
                    		continue;                    		
                    	}
                    	
                    	if (groupDataStart) groupRead++;
                    	if (groupDataStart && (groupRead % 2 == 0)) continue;
                    	
                    	//Put the access value and id in the map of dataset.
                    	AccessInfo accessInfo = new AccessInfo();
                    	accessInfo.setAccess(currentAccessValue);
                    	accessInfo.setId(id);
                    	accessInfo.setUserVsGroup(groupDataStart ? "GRP" : "UID");
                    	if (!pdfDataSets.containsKey(currentDataSet)) {
                    		pdfDataSets.put(currentDataSet, new ArrayList<AccessInfo>());
                    	}
                    	pdfDataSets.get(currentDataSet).add(accessInfo);
                    	
                    	//System.out.println(currentDataSet + " " + currentAccessValue + " " + (groupDataStart ? "GRP" : "UID") + " " + id);
                    	pdfOut.write(currentDataSet + " " + currentAccessValue + " " + (groupDataStart ? "GRP" : "UID") + " " + id);
                    	pdfOut.write("\r\n");
                    	continue;
                    }
                    
	            } //Current Page ends
            }
        }
        finally {
            if( doc != null ) {
                doc.close();
            }
        }
	}
	
	
	static void doTheWordThing() throws IOException {
		POIFSFileSystem pfs = new POIFSFileSystem(new FileInputStream(inputWordFile));
		HWPFDocument wordDoc = new HWPFDocument(pfs);
		Range range = wordDoc.getRange();
		
		boolean section3dot1 = false;
		for (int i=0; i<range.numParagraphs(); i++) {
			Paragraph p = range.getParagraph(i);

			//Anything before section 3.1 is ignored
			if (p.text().startsWith("3.1 MVS Data Sets")) {
				section3dot1 = true;
			}
			
			//The table starts with a cell called DATESET:
			//We should start only if this is section 3.1
			if (p.text().equals("DATASET:" + BEL) && section3dot1) {
				try {
					//Get the table
					Table t = range.getTable(p);
					
					//Iterate the table
					String currentDataSet = null;
					for (int j=1; j<t.numRows(); j++) {
						String dataSet = t.getRow(j).getCell(0).text().trim();

						//There is a space-like character at the start of each empty row
						if (dataSet.trim().length() > 1)
							currentDataSet = dataSet;
						
						String userGrpAcc = t.getRow(j).getCell(7).text().trim();
						String groupOrUser = t.getRow(j).getCell(8).text().trim();
						
						//System.out.println(currentDataSet + " " + userGrpAcc + " " + groupOrUser);
						wordOut.write(currentDataSet + " " + userGrpAcc + " " + groupOrUser);
						wordOut.write("\r\n");
					}
				} catch (Exception e) {
					//This is some other text which starts with DATASET:
					//It is not the one in the table. Just ignore it
				}
			}
			
			//Anything after section 3.1 is ignored
			if (p.text().startsWith("3.2 CICS Transactions")) {
				section3dot1 = false;
			}
		}
		
	}
}
