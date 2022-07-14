CREATE function LPAD  
(  
 @pad_value varchar(500),  
 @pad_length int,  
 @pad_with varchar(10)  
)  
returns varchar(5000)  
as  
BEGIN  
 Declare @value_result varchar(5000)  
 select @value_result= replace(str(@pad_value,@pad_length),' ',@pad_with)   
 return @value_result  
END