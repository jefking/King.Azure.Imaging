King.Azure.Imaging is made for your Azure PaaS environment; deploy to [Azure Web App](http://azure.microsoft.com/en-us/services/app-service/web/) and enable image upload, processing, and delivery!
+ Architecture [Video](https://channel9.msdn.com/Series/onacloud/Cloud-Image-Processing)
+ Upload images to BLOB storage
+ WebJob background image processing
+ API for structured metadata about images
+ API to resize on the fly (and caches!)
+ Azure CDN for low latency content delivery
+ Scalable architecture, all on PaaS
+ Extensible
+ Uses [ImageProcessor](https://github.com/JimBobSquarePants/ImageProcessor) for image maniputlation
+ Uses [King.Azure](https://github.com/jefking/King.Azure) for Azure Storage access
+ Integrates with [King.Service](https://github.com/jefking/King.Service) for background processing

### [NuGet](https://www.nuget.org/packages/King.Azure.Imaging)
```
PM> Install-Package King.Azure.Imaging
```

### Demo
Check out the demo [MVC/Web API project](https://github.com/jefking/King.Azure.Imaging/tree/master/King.Azure.Imaging.Mvc) to help you get started! Or [Live Demo](https://azureimaging.azurewebsites.net/)

### [Wiki](https://github.com/jefking/King.Azure.Imaging/wiki)
View the wiki to learn how to use this.