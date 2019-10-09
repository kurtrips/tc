package com.topcoder.sample.pptx.dto;

public class SummaryOfPerformanceMetricData {
	Long id;
	String calculation;
	String measure;
	Double yourScore;
	Double yourPercentile;
	Double median;
	Double benchmark;
	Long sampleSize;
	
	public Long getId() {
		return id;
	}
	public void setId(Long id) {
		this.id = id;
	}
	public String getCalculation() {
		return calculation;
	}
	public void setCalculation(String calculation) {
		this.calculation = calculation;
	}
	public String getMeasure() {
		return measure;
	}
	public void setMeasure(String measure) {
		this.measure = measure;
	}
	public Double getYourScore() {
		return yourScore;
	}
	public void setYourScore(Double yourScore) {
		this.yourScore = yourScore;
	}
	public Double getYourPercentile() {
		return yourPercentile;
	}
	public void setYourPercentile(Double yourPercentile) {
		this.yourPercentile = yourPercentile;
	}
	public Double getMedian() {
		return median;
	}
	public void setMedian(Double median) {
		this.median = median;
	}
	public Double getBenchmark() {
		return benchmark;
	}
	public void setBenchmark(Double benchmark) {
		this.benchmark = benchmark;
	}
	public Long getSampleSize() {
		return sampleSize;
	}
	public void setSampleSize(Long sampleSize) {
		this.sampleSize = sampleSize;
	}
}
