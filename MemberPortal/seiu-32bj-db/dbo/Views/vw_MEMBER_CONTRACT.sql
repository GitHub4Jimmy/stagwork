
CREATE VIEW [dbo].[vw_MEMBER_CONTRACT]
AS
SELECT
 mc.member_id
,mc.member_contract_id
,mc.start_date hire_date
,mc.stop_date term_date
,mc.certified_leave_stop_date Termination_Receive_Date
,CASE mc.member_contract_status WHEN 'A' THEN 'Active' WHEN 'O' THEN 'On Leave' WHEN 'T' THEN 'Terminated' ELSE member_contract_status END AS current_status
,e.employer_id
,e.employer_name
,be.billing_id
,be.billing_entity_name
,bu.barg_unit_id
,bu.barg_unit_name
,(SELECT   av.description
 FROM dbo.attribute_value av,
      dbo.barg_unit bu1
 WHERE av.attribute_id=7895
 AND bu1.barg_unit_id  = bu.barg_unit_id
 AND av.internal_value= bu.barg_unit_type) Agreement_TYPE
,wl.work_location_id
,wl.work_location_name
,jc.job_category
,jc.category_name
,jcl.job_class
,jcl.job_description
,mc.voting_status
,mc.date_01 Pension_Experience_Date
,mc.date_02 Profit_Sharing_Experience_Date
,mc.date_03 Overr_Pension_Exp_Date
,mc.date_04 Overr_Profit_Sharing_Exp_Date
,mc.date_08 two_yr_Resident_SRSP_Supp_Date
,mc.member_contract_flag_02 Waive_Ifees
,ISNULL(mc.member_contract_flag_15,'N') Union_Dues_Inv_Priority_Flag
,mc.inserted_date
,mc.inserted_by
,mc.updated_date
,mc.updated_by updated_by
,row_number() over (PARTITION BY mc.member_id ORDER BY CASE WHEN stop_date IS NULL THEN 1 ELSE 0 END DESC,member_contract_id desc) AS rn
FROM dbo.member_contract mc
     JOIN dbo.billing_entity be ON mc.billing_id = be.billing_id
     JOIN dbo.work_location  wl ON be.work_location_id = wl.work_location_id
	 JOIN dbo.barg_unit bu ON mc.barg_unit_id = bu.barg_unit_id
     JOIN dbo.employer e ON mc.employer_id = e.employer_id
     LEFT JOIN dbo.job_category jc    ON mc.job_category = jc.job_category     
	 LEFT JOIN dbo.job_class jcl ON mc.job_class_id = jcl.job_class