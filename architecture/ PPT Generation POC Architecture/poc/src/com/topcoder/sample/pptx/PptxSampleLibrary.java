/**
 * 
 */
package com.topcoder.sample.pptx;

import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.List;
import java.util.UUID;

import javax.xml.bind.JAXBElement;
import javax.xml.bind.JAXBException;

import org.docx4j.XmlUtils;
import org.docx4j.dml.CTNonVisualDrawingProps;
import org.docx4j.dml.CTPoint2D;
import org.docx4j.dml.CTRegularTextRun;
import org.docx4j.dml.CTTable;
import org.docx4j.dml.CTTableCell;
import org.docx4j.dml.CTTableRow;
import org.docx4j.dml.CTTextBody;
import org.docx4j.dml.CTTextParagraph;
import org.docx4j.dml.ObjectFactory;
import org.docx4j.dml.chart.CTBarChart;
import org.docx4j.dml.chart.CTBarSer;
import org.docx4j.dml.chart.CTChartSpace;
import org.docx4j.dml.chart.CTNumVal;
import org.docx4j.dml.chart.CTRadarChart;
import org.docx4j.dml.chart.CTRadarSer;
import org.docx4j.dml.chart.CTRelId;
import org.docx4j.openpackaging.exceptions.Docx4JException;
import org.docx4j.openpackaging.exceptions.InvalidFormatException;
import org.docx4j.openpackaging.packages.OpcPackage;
import org.docx4j.openpackaging.packages.PresentationMLPackage;
import org.docx4j.openpackaging.packages.SpreadsheetMLPackage;
import org.docx4j.openpackaging.parts.Part;
import org.docx4j.openpackaging.parts.PartName;
import org.docx4j.openpackaging.parts.DrawingML.Chart;
import org.docx4j.openpackaging.parts.PresentationML.MainPresentationPart;
import org.docx4j.openpackaging.parts.PresentationML.NotesSlidePart;
import org.docx4j.openpackaging.parts.PresentationML.SlideLayoutPart;
import org.docx4j.openpackaging.parts.PresentationML.SlidePart;
import org.docx4j.openpackaging.parts.SpreadsheetML.WorksheetPart;
import org.docx4j.openpackaging.parts.WordprocessingML.EmbeddedPackagePart;
import org.docx4j.openpackaging.parts.relationships.RelationshipsPart.AddPartBehaviour;
import org.docx4j.relationships.Relationship;
import org.pptx4j.pml.CTGraphicalObjectFrame;
import org.pptx4j.pml.GroupShape;
import org.pptx4j.pml.Presentation.SldIdLst.SldId;
import org.pptx4j.pml.Shape;
import org.pptx4j.pml.Sld;
import org.xlsx4j.sml.Row;

import com.topcoder.sample.pptx.dto.AnswerValidationMessageData;
import com.topcoder.sample.pptx.dto.BusinessDriverData;
import com.topcoder.sample.pptx.dto.CostMeasureData;
import com.topcoder.sample.pptx.dto.PerformanceTriangleData;
import com.topcoder.sample.pptx.dto.SummaryOfPerformanceMetricData;
import com.topcoder.sample.pptx.util.ByteBufferBackedInputStream;


/**
 * @author TCSASSEMBLER
 *
 */
public class PptxSampleLibrary {
	
	/*
	 * Used to create objects. 
	 * Not really necessary to use if the objects have default ctors
	 */
	static ObjectFactory objectFactory = new ObjectFactory();
	
	/*
	 * Holds the source pptx file. 
	 */
	PresentationMLPackage presentationMLPackage;
	
	/*
	 * Loads the source pptx 
	 */
	public PptxSampleLibrary() throws Docx4JException {
		this.presentationMLPackage = (PresentationMLPackage)OpcPackage.load(
				new java.io.File("test_files/POC_Template_short.pptx"));
	}

	/*
	 * Creates a new slide in the pptx and returns it
	 */
	private SlidePart createNewSlide(PresentationMLPackage targetPkg, int sourceSlideNum) 
			throws InvalidFormatException, JAXBException {
		
		MainPresentationPart pp = (MainPresentationPart)targetPkg.getParts().getParts().get(
				new PartName("/ppt/presentation.xml"));
		
		SlideLayoutPart layoutPart = null;
		if (sourceSlideNum == 1)
			layoutPart = (SlideLayoutPart)targetPkg.getParts().getParts().get(
				new PartName("/ppt/slideLayouts/slideLayout2.xml"));
		else if (sourceSlideNum == 2)
			layoutPart = (SlideLayoutPart)targetPkg.getParts().getParts().get(
					new PartName("/ppt/slideLayouts/slideLayout12.xml"));			
		else if (sourceSlideNum == 3)
			layoutPart = (SlideLayoutPart)targetPkg.getParts().getParts().get(
					new PartName("/ppt/slideLayouts/slideLayout7.xml"));
		else if (sourceSlideNum == 4)
			layoutPart = (SlideLayoutPart)targetPkg.getParts().getParts().get(
					new PartName("/ppt/slideLayouts/slideLayout13.xml"));
		else if (sourceSlideNum == 5)
			layoutPart = (SlideLayoutPart)targetPkg.getParts().getParts().get(
					new PartName("/ppt/slideLayouts/slideLayout14.xml"));
		
		Iterator<Part> partsIt = targetPkg.getParts().getParts().values().iterator();
		Integer slidesAdded = 1; //To account for the already existing 5 slides
		while (partsIt.hasNext()) {
			if (partsIt.next().getClass().equals(SlidePart.class))
				slidesAdded++;
		}
		String slideToAddPartName = "/ppt/slides/slide" + slidesAdded.toString() + ".xml";
		// OK, now we can create a slide
		return PresentationMLPackage.createSlidePart(pp, layoutPart, new PartName(slideToAddPartName));
		
	}
	
	/*
	 * Put data in the first slide and add it to output pptx
	 */
	public void addAnswerValidationSlide(AnswerValidationMessageData data) 
					throws InvalidFormatException, JAXBException {
		
		//Sample code for replacing text
		//Assumes a fixed position and structure of the Shape into which the data is to be added. 
		SlidePart slide = (SlidePart)presentationMLPackage.getParts().get(
				new PartName("/ppt/slides/slide1.xml") );

		//Clone the existing slide and add it a new slide to pptx 
		Sld sld = XmlUtils.deepCopy(slide.getJaxbElement(), slide.getJAXBContext());
		SlidePart targetSlide = createNewSlide(presentationMLPackage, 1);
		targetSlide.setJaxbElement(sld);
		
		//The shape containing the template text is called "Rectangle 110" 
		Shape shape = (Shape) findSpOrGrpSpOrGraphicFrameByName(targetSlide, "Rectangle 110");
		
		//Only the first text run is to be used
		List<Object> textRuns = shape.getTxBody().getP().get(0).getEGTextRun();		
		CTRegularTextRun textRun = (CTRegularTextRun) textRuns.get(0);
		textRun.setT(data.getAnswerValidationMessagesText());
		
		//Other text runs are to be deleted
		for (int i=textRuns.size() - 1; i>0; i--)
			textRuns.remove(i);
	}
	
	/*
	 * Put data in the second slide and add it to output pptx
	 */
	public void addSummaryOfPerformanceSlide(List<SummaryOfPerformanceMetricData> data) 
			throws InvalidFormatException, JAXBException {
		//Sample code for adding row to an existing table
		SlidePart slide = (SlidePart)presentationMLPackage.getParts().get(
				new PartName("/ppt/slides/slide2.xml") );
		
		Sld sld = XmlUtils.deepCopy(slide.getJaxbElement(), slide.getJAXBContext());
		SlidePart targetSlide = createNewSlide(presentationMLPackage, 2);
		targetSlide.setJaxbElement(sld);		
		
		//The CTGraphicalObjectFrame containing the table is called "Group 164"
		CTGraphicalObjectFrame theTable = 
				(CTGraphicalObjectFrame) findSpOrGrpSpOrGraphicFrameByName(targetSlide, "Group 164");
		
		JAXBElement<CTTable> tableData = 
				(JAXBElement<CTTable>) theTable.getGraphic().getGraphicData().getAny().get(0);

		//Get the second row (empty row) and use it as reference
		CTTableRow refRow = tableData.getValue().getTr().get(2);
		for (int i=0; i<data.size(); i++ ) {
			CTTableRow newRow = XmlUtils.deepCopy(refRow, slide.getJAXBContext());
			newRow.getTc().set(0, createTableCell(
					newRow.getTc().get(0), targetSlide, data.get(i).getId().toString()));
			newRow.getTc().set(1, createTableCell(
					newRow.getTc().get(1), targetSlide, data.get(i).getCalculation()));			
			newRow.getTc().set(2, createTableCell(
					newRow.getTc().get(2), targetSlide, data.get(i).getMeasure()));
			newRow.getTc().set(3, createTableCell(
					newRow.getTc().get(3), targetSlide, data.get(i).getYourScore().toString()));
			newRow.getTc().set(4, createTableCell(
					newRow.getTc().get(4), targetSlide, data.get(i).getYourPercentile().toString()));
			newRow.getTc().set(5, createTableCell(
					newRow.getTc().get(5), targetSlide, data.get(i).getMedian().toString()));
			newRow.getTc().set(6, createTableCell(
					newRow.getTc().get(6), targetSlide, data.get(i).getBenchmark().toString()));
			newRow.getTc().set(7, createTableCell(
					newRow.getTc().get(7), targetSlide, data.get(i).getSampleSize().toString()));			
			tableData.getValue().getTr().add(newRow);
		}
		tableData.getValue().getTr().remove(refRow);
	}
	
	/*
	 * Put data in the third slide and add it to output pptx
	 */
	public void addPerformanceTriangleSlide(PerformanceTriangleData data) 
			throws JAXBException, IOException, Docx4JException {
		
		//Sample code for changing chart data (radar chart)
		SlidePart slide = (SlidePart)presentationMLPackage.getParts().get(
				new PartName("/ppt/slides/slide3.xml") );
		
		//Create a clone of the existing slide
		Sld sld = XmlUtils.deepCopy(slide.getJaxbElement(), slide.getJAXBContext());
		SlidePart targetSlide = createNewSlide(presentationMLPackage, 3);
		targetSlide.setJaxbElement(sld);		
		
		//The CTGraphicalObjectFrame containing the chart is called "chart_perftri"
		CTGraphicalObjectFrame chartParent = 
				(CTGraphicalObjectFrame) findSpOrGrpSpOrGraphicFrameByName(targetSlide, "chart_perftri");
		//Get the chart based on the chart relationship id
		JAXBElement<CTRelId> chartRelId = (JAXBElement<CTRelId>) chartParent.getGraphic().getGraphicData().getAny().get(0);
		Chart chart1Part = (Chart) slide.relationships.getPart(chartRelId.getValue().getId());
		
		//Get the actual radar chart in the chart space
		CTRadarChart radarChart = (CTRadarChart)chart1Part.getJaxbElement().
				getChart().getPlotArea().getAreaChartOrArea3DChartOrLineChart().get(0);
		
		//Write the chart data
		for (CTRadarSer ser : radarChart.getSer()) {
			List<CTNumVal> points = ser.getVal().getNumRef().getNumCache().getPt();
			if (ser.getOrder().getVal() == 0) {
				for (CTNumVal point : points) {
					if (point.getIdx() == 0)
						point.setV(data.getYourCost().toString());
					else if (point.getIdx() == 1)
						point.setV(data.getYourCycleTime().toString());
					else if (point.getIdx() == 2)
						point.setV(data.getYourEfficiencyQuality().toString());
				}
			}
			if (ser.getOrder().getVal() == 1) {
				for (CTNumVal point : points) {
					if (point.getIdx() == 0)
						point.setV(data.getIdealCost().toString());
					else if (point.getIdx() == 1)
						point.setV(data.getIdealCycleTime().toString());
					else if (point.getIdx() == 2)
						point.setV(data.getIdealEfficiencyQuality().toString());
				}
			}			
		}
		
		//Must also update the underlying XLS sheet.
		//Load the workbook
		EmbeddedPackagePart chartDataSheet = (EmbeddedPackagePart)chart1Part.relationships.getPart(
				chart1Part.relationships.getJaxbElement().getRelationship().get(0));
		ByteBufferBackedInputStream bbbis = new ByteBufferBackedInputStream(chartDataSheet.getBuffer());
		//Load the sheet
		SpreadsheetMLPackage spreadsheetMLPkg = (SpreadsheetMLPackage)OpcPackage.load(bbbis);
		WorksheetPart worksheetPart = (WorksheetPart)
				spreadsheetMLPkg.getParts().get(new PartName("/xl/worksheets/sheet1.xml"));
		
		//Update the sheet data
		int i = 0;
		List<Row> sheetRows = worksheetPart.getJaxbElement().getSheetData().getRow();
		for (Row row : sheetRows) {
			if (i == 1) {
				row.getC().get(1).setV(data.getYourCost().toString());
				row.getC().get(2).setV(data.getYourCycleTime().toString());
				row.getC().get(3).setV(data.getYourEfficiencyQuality().toString());
			}
			if (i == 3) {
				row.getC().get(1).setV(data.getIdealCost().toString());
				row.getC().get(2).setV(data.getIdealCycleTime().toString());
				row.getC().get(3).setV(data.getIdealEfficiencyQuality().toString());
			}			
			i++;
		}
		//Write back to workbook
		File tempFile = File.createTempFile(UUID.randomUUID().toString(), null);
		spreadsheetMLPkg.save(tempFile);
		chartDataSheet.setBinaryData(Files.readAllBytes(tempFile.toPath()));
		tempFile.delete();
		
	
		//This is weird. There seems a cyclic relationship between notesSlide1.xml and slide3.xml which we will remove
		NotesSlidePart notesSlide1Part = (NotesSlidePart)presentationMLPackage.getParts().get(
				new PartName("/ppt/notesSlides/notesSlide1.xml"));
		Relationship fromNotesSlide1ToSlide3 = notesSlide1Part.relationships.getRelationshipByID("rId2");
		notesSlide1Part.relationships.removeRelationship(fromNotesSlide1ToSlide3);
		
		//Add the notes 
		targetSlide.addTargetPart(notesSlide1Part);
		
		//Add the chart
		Relationship chartRel = targetSlide.addTargetPart(chart1Part, AddPartBehaviour.RENAME_IF_NAME_EXISTS);
	}
	
	/*
	 * Put data in the fourth slide and add it to output pptx
	 */
	public void addCostMeasureSlide(CostMeasureData data) throws InvalidFormatException, JAXBException {
		
		//Sample code for changing the line data points		
		SlidePart slide = (SlidePart)presentationMLPackage.getParts().get(
				new PartName("/ppt/slides/slide4.xml") );
		
		Sld sld = XmlUtils.deepCopy(slide.getJaxbElement(), slide.getJAXBContext());
		SlidePart targetSlide = createNewSlide(presentationMLPackage, 4);
		targetSlide.setJaxbElement(sld);		
		
		//The score and percentile text
		Shape scorePercentileShape = (Shape) findSpOrGrpSpOrGraphicFrameByName(targetSlide, "txt_your_score_label");
		CTRegularTextRun scorePercentileTextRun = (CTRegularTextRun)scorePercentileShape.getTxBody().getP().get(0).getEGTextRun().get(0);
		String scorePercentileText = 
				scorePercentileTextRun.getT().
					replace("<score />", data.getYourScore().toString()).
					replace("<percentile/>", data.getYourPercentile().toString());
		scorePercentileTextRun.setT(scorePercentileText);
		
		//The line
		Shape lineShape = (Shape) findSpOrGrpSpOrGraphicFrameByName(targetSlide, "lin_7");
		CTPoint2D lineStartPos = lineShape.getSpPr().getXfrm().getOff();
		long lineLength = lineShape.getSpPr().getXfrm().getExt().getCx();
		//Calculate the position of user score, median and benchmark (assuming 15 min and 100 max)
		//Position the user score, median and benchmark
		Shape userScoreShape = (Shape) findSpOrGrpSpOrGraphicFrameByName(targetSlide, "sybl_site_7");
		userScoreShape.getSpPr().getXfrm().getOff().setX(
				lineStartPos.getX() + (long)((data.getYourScore() - 15) / 85.0 * lineLength));
		Shape benchmarkShape = (Shape) findSpOrGrpSpOrGraphicFrameByName(targetSlide, "sybl_perc_7");
		benchmarkShape.getSpPr().getXfrm().getOff().setX(
				lineStartPos.getX() + (long)((data.getBenchmarkScore() - 15) / 85.0 * lineLength));
		Shape medianShape = (Shape) findSpOrGrpSpOrGraphicFrameByName(targetSlide, "sybl_med_7");
		medianShape.getSpPr().getXfrm().getOff().setX(
				lineStartPos.getX() + (long)((data.getMedianScore() - 15) / 85.0 * lineLength));
	}
	
	/*
	 * Put data in the fifth slide and add it to output pptx
	 */
	public void addBusinessDriverSlide(BusinessDriverData data) 
			throws IOException, Docx4JException, JAXBException {
		
		//Sample code for changing chart data (bar chart)
		SlidePart slide = (SlidePart)presentationMLPackage.getParts().get(
				new PartName("/ppt/slides/slide5.xml") );	
		
		Sld sld = XmlUtils.deepCopy(slide.getJaxbElement(), slide.getJAXBContext());
		SlidePart targetSlide = createNewSlide(presentationMLPackage, 5);
		targetSlide.setJaxbElement(sld);
		
		//The CTGraphicalObjectFrame containing the chart is called "chart_1"
		CTGraphicalObjectFrame chartParent = 
				(CTGraphicalObjectFrame) findSpOrGrpSpOrGraphicFrameByName(targetSlide, "chart_1");
		//Get the chart based on the chart relationship id
		JAXBElement<CTRelId> chartRelId = (JAXBElement<CTRelId>) chartParent.getGraphic().getGraphicData().getAny().get(0);
		Chart chart2Part = (Chart) slide.relationships.getPart(chartRelId.getValue().getId());
		
		CTBarChart barChart = (CTBarChart)chart2Part.getJaxbElement().getChart().getPlotArea().getAreaChartOrArea3DChartOrLineChart().get(0);
		int i = 0;
		for (CTBarSer ser : barChart.getSer()) {
			CTNumVal userPoint = ser.getVal().getNumRef().getNumCache().getPt().get(9);
			userPoint.setV(data.getYourCategoryScores().get(i).toString());
			i++;
		}		
		
		//Must also update the underlying XLS sheet.
		//Load the workbook
		EmbeddedPackagePart chartDataSheet = (EmbeddedPackagePart)chart2Part.relationships.getPart(
				chart2Part.relationships.getJaxbElement().getRelationship().get(0));
		ByteBufferBackedInputStream bbbis = new ByteBufferBackedInputStream(chartDataSheet.getBuffer());
		//Load the sheet
		SpreadsheetMLPackage spreadsheetMLPkg = (SpreadsheetMLPackage)OpcPackage.load(bbbis);
		WorksheetPart worksheetPart = (WorksheetPart)
				spreadsheetMLPkg.getParts().get(new PartName("/xl/worksheets/sheet1.xml"));
		i = 0;
		List<Row> sheetRows = worksheetPart.getJaxbElement().getSheetData().getRow();
		for (Row row : sheetRows) {
			if (row.getR() == 1 || row.getR() == 9)
				continue;
			row.getC().get(row.getC().size() - 1).setV(data.getYourCategoryScores().get(i).toString());
			i++;
		}
		//Write back to workbook
		File tempFile = File.createTempFile(UUID.randomUUID().toString(), null);
		spreadsheetMLPkg.save(tempFile);
		chartDataSheet.setBinaryData(Files.readAllBytes(tempFile.toPath()));
		tempFile.delete();
		
		//This is weird. There seems a cyclic relationship between notesSlide3.xml and slide5.xml which we will remove
		NotesSlidePart notesSlide3Part = (NotesSlidePart)presentationMLPackage.getParts().get(
				new PartName("/ppt/notesSlides/notesSlide3.xml"));
		Relationship fromNotesSlide3ToSlide5 = notesSlide3Part.relationships.getRelationshipByID("rId2");
		notesSlide3Part.relationships.removeRelationship(fromNotesSlide3ToSlide5);
		
		//Also explicitly add the relationship to the notes and charts 
		targetSlide.addTargetPart(notesSlide3Part);		
		targetSlide.addTargetPart(chart2Part);		
	}
	
	/*
	 * Saves the output pptx to the destination path
	 */
	public void save(String destinationPath)
			throws Docx4JException {

		//Prevent the existing slides from showing up
		for (int i=4; i>=0; i--)  {
			SldId sldId = presentationMLPackage.getMainPresentationPart().getJaxbElement().
					getSldIdLst().getSldId().get(i);
			
			presentationMLPackage.getMainPresentationPart().getJaxbElement().
					getSldIdLst().getSldId().remove(i);

			Relationship r = presentationMLPackage.getMainPresentationPart().
				relationships.getRelationshipByID(sldId.getRid());
			
			presentationMLPackage.getMainPresentationPart().relationships.removeRelationship(r);
			
			PartName partToRemove = new PartName("/ppt/slides/slide" + new Integer(i+1).toString() + ".xml");
			presentationMLPackage.getParts().remove(partToRemove);
		}
		
		presentationMLPackage.save(new File(destinationPath));
	}
	
	/**
	 * Demo of the POC 
	 */
	public static void main(String[] args) throws Docx4JException, IOException, JAXBException {
		
		PptxSampleLibrary pptxSampleLibrary = new PptxSampleLibrary();
		
		//Add Slide 1
		AnswerValidationMessageData data = new AnswerValidationMessageData();
		data.setAnswerValidationMessagesText("This is some sample answer validation text");
		pptxSampleLibrary.addAnswerValidationSlide(data);
		
		//Add Slide 1 again
		data.setAnswerValidationMessagesText("This is another slide 1!!!");
		pptxSampleLibrary.addAnswerValidationSlide(data);
		
		//Add Slide 2 now
		SummaryOfPerformanceMetricData sopmd = new SummaryOfPerformanceMetricData();
		sopmd.setId(1L);
		sopmd.setCalculation("Some Calc");
		sopmd.setMeasure("Some Measure");
		sopmd.setYourScore(37.0);
		sopmd.setYourPercentile(45.0);
		sopmd.setMedian(50.0);
		sopmd.setBenchmark(60.0);
		sopmd.setSampleSize(1000L);
		List<SummaryOfPerformanceMetricData> sopmdlist = new ArrayList<SummaryOfPerformanceMetricData>();
		sopmdlist.add(sopmd);
		pptxSampleLibrary.addSummaryOfPerformanceSlide(sopmdlist);
		
		//Add Slide 3
		PerformanceTriangleData ptd = new PerformanceTriangleData();
		ptd.setIdealCost(80.);
		ptd.setIdealCycleTime(80.);
		ptd.setIdealEfficiencyQuality(90.);
		ptd.setYourCost(40.);
		ptd.setYourCycleTime(70.);
		ptd.setYourEfficiencyQuality(70.);
		pptxSampleLibrary.addPerformanceTriangleSlide(ptd);
		
		//Add Slide 4
		CostMeasureData cmd = new CostMeasureData();
		cmd.setYourScore(27.0);
		cmd.setYourPercentile(35.0);
		cmd.setBenchmarkScore(60.0);
		cmd.setMedianScore(50.0);
		pptxSampleLibrary.addCostMeasureSlide(cmd );
		
		//Add Slide 5
		BusinessDriverData bds = new BusinessDriverData();
		bds.getYourCategoryScores().add(0.1);
		bds.getYourCategoryScores().add(0.2);
		bds.getYourCategoryScores().add(0.3);
		bds.getYourCategoryScores().add(0.4);
		bds.getYourCategoryScores().add(0.5);
		bds.getYourCategoryScores().add(0.6);
		bds.getYourCategoryScores().add(0.7);
		pptxSampleLibrary.addBusinessDriverSlide(bds);
		
		//Add Slide 4 again
		cmd.setYourScore(57.0);
		cmd.setYourPercentile(65.0);
		cmd.setBenchmarkScore(75.0);
		cmd.setMedianScore(50.0);
		pptxSampleLibrary.addCostMeasureSlide(cmd);
		
		pptxSampleLibrary.save("test_files/pptx-out.pptx");
	}
	
	/**
	 * Convenience method to duplicate an existing table cell.
	 */
	private static CTTableCell createTableCell(CTTableCell existing, SlidePart slide, String value) {
		//Clone the existing cell
		CTTableCell newCell = XmlUtils.deepCopy(existing, slide.getJAXBContext());

		//Get the text para of the new cell 
		CTTextParagraph textPara = newCell.getTxBody().getP().get(0);
		
		//Create a new text run with the specified text
		CTRegularTextRun textRun = objectFactory.createCTRegularTextRun();
		textRun.setT(value);
		
		//This is what sets the fonts to the existing cell font
		textRun.setRPr(textPara.getEndParaRPr());

		//Add the text run to para and return
		textPara.getEGTextRun().add(textRun);
		return newCell;
	}

	/*
	 * Finds a shape based on the name defined in the non visual shape properties
	 */
	private static Object findSpOrGrpSpOrGraphicFrameByName(SlidePart slide, String name) throws JAXBException {

		List<Object> shapesOrGrpShpOrGrphcFrms = 
				slide.getJaxbElement().getCSld().getSpTree().getSpOrGrpSpOrGraphicFrame();
		for (Object obj : shapesOrGrpShpOrGrphcFrms) {
			if (obj.getClass().equals(Shape.class)) {
				Shape shp = (Shape) obj;
				if (shp.getNvSpPr().getCNvPr().getName().equals(name))
					return shp;
			}
			else if (obj.getClass().equals(GroupShape.class)) {
				GroupShape shp = (GroupShape) obj;
				if (shp.getNvGrpSpPr().getCNvPr().getName().equals(name))
					return shp;
			}
			else if (obj.getClass().equals(CTGraphicalObjectFrame.class)) {
				CTGraphicalObjectFrame shp = (CTGraphicalObjectFrame) obj;
				if (shp.getNvGraphicFramePr().getCNvPr().getName().equals(name))
					return shp;
			}
			else
				return null;
		}
		return null;
	}
}
