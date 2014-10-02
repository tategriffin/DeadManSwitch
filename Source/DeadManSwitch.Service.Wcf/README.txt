Error: The target assembly contains no service types. You may need to adjust the code access security.
http://stackoverflow.com/questions/564541/how-can-i-get-rid-of-the-the-target-assembly-contains-no-service-types-error-m
	In the WCF Options tab of the properties of the project defining the ServiceContract, there's a checkbox labelled 
	"Start WCF Service Host when debugging another project in the same solution" that I unchecked.
