Verndale Sitecore 7 Fallback Glass Mapper Updates for Sitecore Language Fallback

Purpose
Update Glass Mapper to take language fallback into account when generating indexes.

Compatibility
This is for use with Sitecore 7 and Glass Mapper V3
https://github.com/sitecorian/SitecoreSearchContrib
It has been verified to work with Sitecore 7.2 update 2
And it assumes you are using the latest version of the Partial Language Fallback Module
http://marketplace.sitecore.net/en/Modules/Language_Fallback.aspx

How to build code and deploy the solution
1. When using the fallback module, make sure to compile the source code to get the dll, do not use the one within the package.  Make sure to compile against the version of the Sitecore Kernel and Client dlls that you will be using for your main project

2. Add reference to the Sitecore.SharedSource.PartialLanguageFallback.dll that you compiled

3. Add Sitecore.Context.Items["Disable"] = new VersionCountDisabler(); to the Application_BeginRequest method in Global.asax

4. Put the FallbackCheckTask.cs in your project

5. Update GlassMapperScCustom.cs to register the FallbackCheckTask within the CastleConfig method


Testing
1. There should be more than one language in your site 
2. Add a version to an item for some languages, but not others
3. Leave enforce version presence configured to false
4. Publish to languages for which you have created versions of the item
5. Switch to the language that does NOT have a version in that language
6. Make sure you are using Glass Mapper to get an item and output its values
6. Verify it falls back successfully and does not result in a null item

Review the blog series about Partial Language Fallback on Sitecore, http://www.sitecore.net/en-gb/Learn/Blogs/Technical-Blogs/Elizabeth-Spranzani.aspx
