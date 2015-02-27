# TeamCity Change Notifier

After seeing another organisation where after a successful deploy, a release email was automatically sent to all team members summarising the commits that went into the release, I thought "That's nice, why can't we have that?"

As it turns out, our deployment infrastructure is more *sophisticated* so it's harder. But you can use the [TeamCity rest api](https://confluence.jetbrains.com/display/TCD8/REST+API) to find the changes in a release and send a notification. 

This involves walking along the TeamCity rest Api a bit, since this build doesn't itself contain the changes, it releases artefacts from an earlier build which is a dependency. The "release" is the last build in the chain., The first in the chain, the "build and unit test" actually gets the code changes. And the changes being released are spread over multiple builds, back to just after the previous pinned build. So we have to:  

1. First read the released build and get the id of the "first build" dependency.
2. Read the details of the  first build, particularly the `id` and `buildTypeId`.
3. Read all the builds in the first build's `buildTypeId`, in order to get a range of builds from this back to just after the previous pinned build.
4. Read changes on all the builds in range.

The steps to get it working are:  
1.  Build this program.  
2.  Configure the `app.config` with suitable values. You should know the teamcity url and smtp settings. The TeamCityAuthInfo [should be generated like the authInfo here](http://stackoverflow.com/a/13706696/5599).  
3. Find the TeamCity internal id of the released build. If you are looking at a build result in a browser, it is the number in the url right after `?buildId=`. In  TeamCity build automation it is `build.number`
 

## Sample output

```
Release to Catalogue with 2 changes in 1 build

anthony.steele on Tue 03/02/2015 15:08
 HttpRequestException as release error not warming

anthony.steele on Tue 03/02/2015 14:50
 Formatting
```

## Caveats

Robustness is not yet guaranteed. Usage will flush this out.

It assumes that the release depends upon the build that contains the changes. And that changes go from this build back to the last pin. It could be made more flexible if need be, but there is no use case for this yet.