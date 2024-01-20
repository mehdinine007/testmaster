create proc Sp_ConfilictedCustomersReport
	as
begin
  	select [id] = nationalcode ,
		   [کد ملی]=nationalcode 
	      ,[تعداد خرید]=count(0) 
	from ConfilictedCustomers 
	where variz >= 1
	group by nationalcode
	having count(0) > 1
end



