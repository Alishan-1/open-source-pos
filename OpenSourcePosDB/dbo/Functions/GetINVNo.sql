CREATE FUNCTION dbo.GetINVNo(@ID int, @PROCESS_DATE DATETIME,@COMPANY_ID INT)   
RETURNS VARCHAR(20)  
AS        
BEGIN         
  
DECLARE @V_NO VARCHAR(20)  
SELECT @V_NO = SUBSTRING(convert(varchar, YEAR(FiscalYearFrom)),3,2) +  SUBSTRING(CONVERT(varchar,YEAR(FiscalYearTo)),3,2) + DBO.LPAD(@ID,5,'0')  
FROM FiscalYearST  
WHERE @PROCESS_DATE BETWEEN FiscalYearFrom AND FiscalYearTo  
AND CompanyID = @COMPANY_ID  
        
RETURN @V_NO        
        
END