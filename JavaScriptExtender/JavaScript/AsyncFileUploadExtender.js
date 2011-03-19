
Type.registerNamespace("JavaScriptControls")

// constructor
JavaScriptControls.AsyncFileUploadExtender = function (element) 
{
    JavaScriptControls.AsyncFileUploadExtender.initializeBase(this, [element]);
    
    this.AsyncFileUploaderId = null;
    this.AsyncCVUploaderId = null;
    this.AsyncVideoUploaderId = null;
    this.AccountId = null;
    this.ButtonIdList = null;
}

JavaScriptControls.AsyncFileUploadExtender.prototype.initialize = function() 
{
    JavaScriptControls.AsyncFileUploadExtender.callBaseMethod(this, "initialize");
          
    var elementObject = $find(this.AsyncFileUploaderId);    
    elementObject.AccountId = this.AccountId;
    elementObject.ButtonIdList = this.ButtonIdList;

    var elementCVObject = $find(this.AsyncCVUploaderId);    
    elementCVObject.ButtonIdList = this.ButtonIdList;
    
    var elementVideoObject = $find(this.AsyncVideoUploaderId);    
    elementVideoObject.ButtonIdList = this.ButtonIdList;       
}

JavaScriptControls.AsyncFileUploadExtender.prototype.dispose = function() 
{
    $clearHandlers(this.get_element());
    
    JavaScriptControls.AsyncFileUploadExtender.callBaseMethod(this, "dispose");
}

JavaScriptControls.AsyncFileUploadExtender.prototype.get_AsyncFileUploaderId = function()
{
    return this.AsyncFileUploaderId;
}

JavaScriptControls.AsyncFileUploadExtender.prototype.set_AsyncFileUploaderId = function(value)
{
    if (this.AsyncFileUploaderId != value)
    {
        this.AsyncFileUploaderId = value;
        this.raisePropertyChanged("AsyncFileUploaderId");
    }
}

JavaScriptControls.AsyncFileUploadExtender.prototype.get_AsyncCVUploaderId = function()
{
    return this.AsyncCVUploaderId;
}

JavaScriptControls.AsyncFileUploadExtender.prototype.set_AsyncCVUploaderId = function(value)
{
    if (this.AsyncCVUploaderId != value)
    {
        this.AsyncCVUploaderId = value;
        this.raisePropertyChanged("AsyncCVUploaderId");
    }
}

JavaScriptControls.AsyncFileUploadExtender.prototype.get_AsyncVideoUploaderId = function()
{
    return this.AsyncVideoUploaderId;
}

JavaScriptControls.AsyncFileUploadExtender.prototype.set_AsyncVideoUploaderId = function(value)
{
    if (this.AsyncVideoUploaderId != value)
    {
        this.AsyncVideoUploaderId = value;
        this.raisePropertyChanged("AsyncVideoUploaderId");
    }
}

JavaScriptControls.AsyncFileUploadExtender.prototype.get_AccountId = function()
{
    return this.AccountId;
}

JavaScriptControls.AsyncFileUploadExtender.prototype.set_AccountId = function(value)
{
    if (this.AccountId != value)
    {
        this.AccountId = value;
        this.raisePropertyChanged("AccountId");
    }

}

JavaScriptControls.AsyncFileUploadExtender.prototype.get_ButtonIdList = function()
{
    return this.ButtonIdList;
}

JavaScriptControls.AsyncFileUploadExtender.prototype.set_ButtonIdList = function(value)
{
    if (this.ButtonIdList != value)
    {
        this.ButtonIdList = value;
        this.raisePropertyChanged("ButtonIdList");
    }
}

function showError(sender, args)
{
    alert(args._errorMessage);
    
    return true;
}

function DisableControls(sender)
{
    for (i=0; i < sender.ButtonIdList.length; i++)
    {        
        var lControl = $get(sender.ButtonIdList[i]);
        lControl.disabled = true;
    }
}

function EnableControls(sender)
{
    for (i=0; i < sender.ButtonIdList.length; i++)
    {        
        var lControl = $get(sender.ButtonIdList[i]);
        lControl.disabled = false;
    }
}

function SetImagUrl(sender, args)
{
    var lTest = $get('ctl00_ContentPlaceHolder_CandidateEdit_iCandidatePicture');

    if (lTest.src.toLowerCase().lastIndexOf('2.jpg') > 0) 
    {
        lTest.src = "/gfx/CandidatePicture/temp/" + sender.AccountId + "-1.jpg"
    }
    else
    {
        lTest.src = "/gfx/CandidatePicture/temp/" + sender.AccountId + "-2.jpg"
    }
    
    EnableControls(sender)
}

function uploadImageStarted(sender, args) 
{
    DisableControls(sender)
    
    var filename = args.get_fileName();           
    var filext = filename.substring(filename.lastIndexOf(".") + 1).toLowerCase();           
    
    if (filext == "jpg" || filext == "jpeg" || filext == "gif" || filext == "png")      
    {             
        return true;         
    }         
    else          
    {       
        EnableControls(sender)
        var e = { message : "Alleen foto's met de extensie '.jpg', '.jpeg', '.gif' of '.png' kunnen worden ge-upload."};

        throw(e);
    }       
}  

function SetCvUrl(sender, args)
{
    var lTest = $get('ctl00_ContentPlaceHolder_CandidateEdit_lCVStatus');
    var filename = args.get_fileName();
    
    lTest.outerText = filename;    
    
    EnableControls(sender)
}

function uploadCvStarted(sender, args) 
{ 
    DisableControls(sender)
    
    var filename = args.get_fileName();
    var filext = filename.substring(filename.lastIndexOf(".") + 1).toLowerCase();           
    
    if (filext == "pdf")      
    {     
        return true;         
    }         
    else          
    {     
        EnableControls(sender)          
        var e = { message : "Alleen Cv's met de extensie '.pdf' kunnen worden ge-upload."};

        throw(e);
    }       
}  

function SetVideoUrl(sender, args)
{
    debugger;

    var lTest = $get('ctl00_ContentPlaceHolder_CandidateEdit_lVideoStatus');
    var filename = args.get_fileName();
    
    lTest.outerText = filename;    
    
    EnableControls(sender)
}

function uploadVideoStarted(sender, args) 
{      
    debugger;
    DisableControls(sender)

    var filename = args.get_fileName();           
    var filext = filename.substring(filename.lastIndexOf(".") + 1).toLowerCase();           
    
    if (filext == "flv")      
    {             
        return true;         
    }         
    else          
    {        
        EnableControls(sender)       
        var e = { message : "Alleen Video's met de extensie '.flv' kunnen worden ge-upload."};

        throw(e);
    }       
}  

JavaScriptControls.AsyncFileUploadExtender.registerClass('JavaScriptControls.AsyncFileUploadExtender', Sys.UI.Control);

Sys.Application.notifyScriptLoaded();

