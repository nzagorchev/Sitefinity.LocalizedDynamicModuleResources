# Sitefinity.LocalizedDynamicModuleResources
Automated localization for Sitefinity Dynamic modules built with Module Builder. Localize the module backend screens - field names etc.

## Functionality
Auto map dynamic module backend fields labels to a custom resource class. This way the labels can be easily localized.

![alt text][img]
[img]: https://github.com/nzagorchev/Sitefinity.LocalizedDynamicModuleResources/blob/master/func.png "Backend view"

## Sample usage
1. Register the resource class:
http://docs.sitefinity.com/for-developers-create-custom-resource-classes
2. Install using the helper:
```cs
MyDynamicModuleResourcesHelper.Install();
```