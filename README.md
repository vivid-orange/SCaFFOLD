# SCaFFOLD
*Structural Calculations Framework*

![GitHub Actions Workflow Status](https://github.com/VividOrange/SCaFFOLD/actions/workflows/on-merge-to-main.yml/badge.svg)
[![codecov](https://codecov.io/gh/VividOrange/SCaFFOLD/graph/badge.svg)](https://codecov.io/gh/VividOrange/SCaFFOLD)

Free, Open and Lightweight Design

<img src="Calcs/resources/CalcSplash.gif" width="300">

SCaFFOLD is a framework designed to make it easy for engineers to write calculations in code once and to have access to those calculations wherever they need them - whether that's accessing them through a Windows app, mobile apps or embedded within CAD or analysis packages. SCaFFOLD is open source and made available under the MIT license.

Any project using the framework will need to reference the CalcCore library. Calculations can be supplied by any library containing classes that implement the ICalc interface from CalcCore.

The repository contains a number of different projects including:
CalcCore - the core classes that enable the framework.
Calcs - Windows desktop application (WPF).
DynamicRelaxation - dynamic relaxation engine, used for animation in Calcs splash screen.
RunInConsole - console app client for CalcCore.
TestCalcs - various calcuations written for the framework.

## Latest release
[![Releasestatus](https://github.com/VividOrange/SCaFFOLD/actions/workflows/on-release.yml/badge.svg)](https://github.com/VividOrange/SCaFFOLD/releases)
[![NuGet Version](https://img.shields.io/nuget/v/VividOrange.SCaFFOLD.Profiles)](https://www.nuget.org/packages?q=Magmaworks.SCaFFOLD)
![Libraries.io dependency status for GitHub repo](https://img.shields.io/librariesio/github/VividOrange/SCaFFOLD)
![GitHub Release Date](https://img.shields.io/github/release-date/VividOrange/SCaFFOLD)


## Contributing 
[![contributions welcome](https://img.shields.io/badge/contributions-welcome-brightgreen.svg?style=flat)](https://github.com/VividOrange/SCaFFOLD/issues)
![GitHub contributors](https://img.shields.io/github/contributors/VividOrange/SCaFFOLD)
![GitHub top language](https://img.shields.io/github/languages/top/VividOrange/SCaFFOLD)
