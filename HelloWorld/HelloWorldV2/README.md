This project shows middleware chaining, and Run() function usage, which is simplified and type-safe compared to Use().
<br/>
Packages:
<br/>
Install-Package Microsoft.Owin
<br/>
KEY TAKEAWAY:
1) Even with the Use method, if you do not call the Invoke method, next component in the pipeline will not run.
2) Therefore, every middleware component has the power to let the pipeline continue or short-circuit the pipeline.