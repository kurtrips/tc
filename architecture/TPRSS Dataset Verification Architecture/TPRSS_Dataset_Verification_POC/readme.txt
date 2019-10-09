A POC is already provided. See the main method of the Main.java. 
Currently the POC prints the dataset data parsed from all 5 PDFs into output/Pdf.txt and the dataset data parsed from the word document into the output/Word.txt
The POC contains only the dataset parsing part (in full), but the intent is to show the usage of Apache POI HWPF for Word parsing and Apache PdfBox for PDF parsing.

The design documents are in the docs folder.

This folder can be imported into Eclipse. To run, place the following jars into the lib folder:
pdfbox-app-1.7.1.jar 
FROM http://www.apache.org/dyn/closer.cgi/pdfbox/1.7.1/pdfbox-app-1.7.1.jar
poi-3.8-20120326.jar and poi-scratchpad-3.8-20120326.jar 
FROM http://www.apache.org/dyn/closer.cgi/poi/release/bin/poi-bin-3.8-20120326.zip

