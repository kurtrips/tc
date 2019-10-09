package com.topcoder.sample.pptx.dto;

import java.util.ArrayList;
import java.util.List;


public class BusinessDriverData {
	Long metricId;
	String csvQuestionIds;
	List<Double> yourCategoryScores = new ArrayList<Double>();
	public Long getMetricId() {
		return metricId;
	}
	public void setMetricId(Long metricId) {
		this.metricId = metricId;
	}
	public String getCsvQuestionIds() {
		return csvQuestionIds;
	}
	public void setCsvQuestionIds(String csvQuestionIds) {
		this.csvQuestionIds = csvQuestionIds;
	}
	public List<Double> getYourCategoryScores() {
		return yourCategoryScores;
	}
	public void setYourCategoryScores(List<Double> yourCategoryScores) {
		this.yourCategoryScores = yourCategoryScores;
	} 
}

