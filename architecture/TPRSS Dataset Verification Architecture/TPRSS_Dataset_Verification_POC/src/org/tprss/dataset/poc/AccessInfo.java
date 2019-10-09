package org.tprss.dataset.poc;

public class AccessInfo {
	private String id;
	private String access;
	private String userVsGroup;
	private boolean visited = false;
	
	public String getId() {
		return id;
	}
	public void setId(String id) {
		this.id = id;
	}
	public String getAccess() {
		return access;
	}
	public void setAccess(String access) {
		this.access = access;
	}
	public String getUserVsGroup() {
		return userVsGroup;
	}
	public void setUserVsGroup(String userVsGroup) {
		this.userVsGroup = userVsGroup;
	}
	public boolean isVisited() {
		return visited;
	}
	public void setVisited(boolean visited) {
		this.visited = visited;
	}
}
