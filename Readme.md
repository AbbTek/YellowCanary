# Yellow Canary Code Challenge

## Assumptions
- The format of the .xslx file has to be the same that the format provided in the example (including typo errors)
- The quarter for Disbursements is calculated base on the 'pay_period_to'
- The complete excel is loaded in memory; this approach may change depending on the size of the files

## Running the App

Go to the directory YellowCanary.Console

```console
dotnet run -- -p "../YellowCanary.Application.Test/TestFiles/Sample Super Data.xlsx"
```
