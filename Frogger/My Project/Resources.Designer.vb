﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("Frogger.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.IO.UnmanagedMemoryStream similar to System.IO.MemoryStream.
        '''</summary>
        Friend ReadOnly Property AllFrogsHome() As System.IO.UnmanagedMemoryStream
            Get
                Return ResourceManager.GetStream("AllFrogsHome", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.IO.UnmanagedMemoryStream similar to System.IO.MemoryStream.
        '''</summary>
        Friend ReadOnly Property Background() As System.IO.UnmanagedMemoryStream
            Get
                Return ResourceManager.GetStream("Background", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property Frog() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Frog", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        '''</summary>
        Friend ReadOnly Property froggericon() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("froggericon", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 10,13,32
        '''0,-1.2,2,-2.8,-0.8,1.5,0,1,-2,1.5,-2,2,0
        '''-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1
        '''37,-1,38,-1,38,-1,39,-1,-1,-1,-1,37,-1,38,-1,38,-1,39,-1,-1,-1,-1,37,-1,38,-1,38,-1,39,-1,-1,-1,37,-1,38,-1,38,-1,39,-1,-1,-1,-1,37,-1,38,-1,38,-1,39,-1,-1,-1,-1,-1,40,-1,41,-1,42,-1,-1,-1,-1
        '''-1,-1,-1,-1,47,-1,47,-1,-1,-1,-1,44,-1,44,-1,-1,-1,-1,44,-1,44,-1,-1,-1,-1 [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property FroggerMaps() As String
            Get
                Return ResourceManager.GetString("FroggerMaps", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property FroggerSheetTransparant() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("FroggerSheetTransparant", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.IO.UnmanagedMemoryStream similar to System.IO.MemoryStream.
        '''</summary>
        Friend ReadOnly Property GotHome() As System.IO.UnmanagedMemoryStream
            Get
                Return ResourceManager.GetStream("GotHome", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.IO.UnmanagedMemoryStream similar to System.IO.MemoryStream.
        '''</summary>
        Friend ReadOnly Property Jump() As System.IO.UnmanagedMemoryStream
            Get
                Return ResourceManager.GetStream("Jump", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property Largefroggerbackground() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Largefroggerbackground", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property LargeFroggerSheet() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("LargeFroggerSheet", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.IO.UnmanagedMemoryStream similar to System.IO.MemoryStream.
        '''</summary>
        Friend ReadOnly Property RoadDead() As System.IO.UnmanagedMemoryStream
            Get
                Return ResourceManager.GetStream("RoadDead", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.IO.UnmanagedMemoryStream similar to System.IO.MemoryStream.
        '''</summary>
        Friend ReadOnly Property sound_frogger_extra() As System.IO.UnmanagedMemoryStream
            Get
                Return ResourceManager.GetStream("sound_frogger_extra", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.IO.UnmanagedMemoryStream similar to System.IO.MemoryStream.
        '''</summary>
        Friend ReadOnly Property sound_frogger_time() As System.IO.UnmanagedMemoryStream
            Get
                Return ResourceManager.GetStream("sound_frogger_time", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.IO.UnmanagedMemoryStream similar to System.IO.MemoryStream.
        '''</summary>
        Friend ReadOnly Property WaterDead() As System.IO.UnmanagedMemoryStream
            Get
                Return ResourceManager.GetStream("WaterDead", resourceCulture)
            End Get
        End Property
    End Module
End Namespace