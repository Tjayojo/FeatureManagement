 @echo off
 
 //Note: Navigate to the root directory of the client project before running 
 
 echo Retrieving swagger definition
 iwr https://localhost:44300/swagger/v1/swagger.json -o ./Docs/OpenAPIs/feature-management-v1.json

 echo Generating Api Client
 autorest --input-file=./Docs/OpenAPIs/feature-management-v1.json --v3 --csharp --add-credentials=true --use-datetimeoffset=true --sync-methods=none --output-folder=./ --namespace=Microsoft.FeatureManagement.Client