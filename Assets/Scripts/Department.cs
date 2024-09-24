using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Department : MonoBehaviour
{
    private enum DepartmentStatus
    {
        STATUS_OFF, STATUS_DECISION, STATUS_STATISTICS
    }
    private DepartmentStatus status;
    private GameObject decisionPanel, statisticsPanel;

    // Start is called before the first frame update
    void Start()
    {
        status = DepartmentStatus.STATUS_OFF;
        GameObject canvas = GameObject.Find("Canvas");
        decisionPanel = canvas.transform.Find("Panel - Decision").gameObject;
        statisticsPanel = canvas.transform.Find("Panel - Statistics").gameObject;
    }

    private GameObject GetDepartmentStatusPanel()
    {
        if (status == DepartmentStatus.STATUS_DECISION)
            return decisionPanel;
        else if (status == DepartmentStatus.STATUS_STATISTICS)
            return statisticsPanel;
        else
            return null;
    }

    // Update is called once per frame
    public void DepartmentButtonClicked()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        string[] departmentNames = clickedButton.name.Split(' ');
        string departmentName = departmentNames[departmentNames.Length - 1];
        if (departmentName == "Decision")
        {
            if (status == DepartmentStatus.STATUS_DECISION)
            {
                decisionPanel.SetActive(false);
                status = DepartmentStatus.STATUS_OFF;
            }
            else if (status == DepartmentStatus.STATUS_OFF)
            {
                status = DepartmentStatus.STATUS_DECISION;
                decisionPanel.SetActive(true);
            }
            else
            {
                GetDepartmentStatusPanel().SetActive(false);
                status = DepartmentStatus.STATUS_DECISION;
                decisionPanel.SetActive(true);
            }
        }
        else if (departmentName == "Statistics")
        {
            if (status == DepartmentStatus.STATUS_STATISTICS)
            {
                statisticsPanel.SetActive(false);
                status = DepartmentStatus.STATUS_OFF;
            }
            else if (status == DepartmentStatus.STATUS_OFF)
            {
                status = DepartmentStatus.STATUS_STATISTICS;
                statisticsPanel.SetActive(true);
            }
            else
            {
                GetDepartmentStatusPanel().SetActive(false);
                status = DepartmentStatus.STATUS_STATISTICS;
                statisticsPanel.SetActive(true);
            }
        }
    }
}
