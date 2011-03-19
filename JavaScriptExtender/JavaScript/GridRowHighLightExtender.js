Type.registerNamespace("JavaScriptControls")

// constructor
JavaScriptControls.GridRowHighLightExtender = function (element) 
{
    JavaScriptControls.GridRowHighLightExtender.initializeBase(this, [element]);
    
    this.HighlightCssClass = null;
    this.NormalCssClass = null;
    this.TargetUrl = null;
}

JavaScriptControls.GridRowHighLightExtender.prototype.initialize = function() 
{
    JavaScriptControls.GridRowHighLightExtender.callBaseMethod(this, "initialize");
    
    var j = 0;
    
    for (var i = 0; i < this.get_element().rows.length; i++)
    {
        if (this.get_element().rows[i].cells[0].tagName != 'TH')
        {
            $addHandlers(this.get_element().rows[i], { 'mouseover': this.onFocus, 'mouseout': this.onBlur , 'click': this.onClick }, this);
            this.get_element().rows[i].style.cursor = 'pointer';
            this.get_element().rows[i].originalColor = this.get_element().rows[i].style.backgroundColor;
            this.get_element().rows[i].index = j++;
        }
    }
}



JavaScriptControls.GridRowHighLightExtender.prototype.dispose = function() 
{
    for (i=0; i < this.get_element().rows.length; i++)
    {
        $clearHandlers(this.get_element().rows[i]);
    }
    
    JavaScriptControls.GridRowHighLightExtender.callBaseMethod(this, "dispose");
}

JavaScriptControls.GridRowHighLightExtender.prototype.onFocus = function(eventElement) 
{
    var tagNode = eventElement.target
    while(tagNode.tagName != 'TR' && tagNode.tagName != 'TH')
    {
        if(!tagNode.disabled && tagNode.tagName == 'TD')
        {
            tagNode.parentNode.originalColor = tagNode.parentNode.style.backgroundColor;
             tagNode.parentNode.style.backgroundColor = '#FFFF00';
        }
        
        tagNode = tagNode.parentNode;
    }
        
//    if(!eventElement.disabled && eventElement.target.tagName != 'TH')
//    {
//        eventElement.target.parentNode.originalColor = eventElement.target.parentNode.style.backgroundColor;
//        
//        eventElement.target.parentNode.style.backgroundColor = '#FFFF00';
//    }
}

JavaScriptControls.GridRowHighLightExtender.prototype.onBlur = function(eventElement) 
{
    var tagNode = eventElement.target
    while(tagNode.tagName != 'TR' && tagNode.tagName != 'TH')
    {
        if(!tagNode.disabled && tagNode.tagName == 'TD')
        {
            tagNode.parentNode.style.backgroundColor = tagNode.parentNode.originalColor;
        }
        
        tagNode = tagNode.parentNode;
    }
    
//    if(!eventElement.disabled && eventElement.target.tagName != 'TH')
//    {           
//        eventElement.target.parentNode.style.backgroundColor = eventElement.target.parentNode.originalColor;
//    }
}

JavaScriptControls.GridRowHighLightExtender.prototype.onClick = function(eventElement) 
{
    if(this.get_element() && !eventElement.disabled && eventElement.target.tagName != 'TH')
    {
        var target = this.TargetUrl + this.IdList[eventElement.target.parentNode.index]
        
        window.open(target, '_self');
    }
}

JavaScriptControls.GridRowHighLightExtender.prototype.get_HighlightCssClass = function()
{
    return this.HighlightCssClass;
}

JavaScriptControls.GridRowHighLightExtender.prototype.set_HighlightCssClass = function(value)
{
    if (this.HighlightCssClass != value)
    {
        this.HighlightCssClass = value;
        this.raisePropertyChanged("HighlightCssClass");
    }
}

JavaScriptControls.GridRowHighLightExtender.prototype.get_NormalCssClass = function()
{
    return this.NormalCssClass;
}

JavaScriptControls.GridRowHighLightExtender.prototype.set_NormalCssClass = function(value)
{
    if (this.NormalCssClass != value)
    {
        this.NormalCssClass = value;
        this.raisePropertyChanged("NormalCssClass");
    }
}

JavaScriptControls.GridRowHighLightExtender.prototype.get_TargetUrl = function()
{
    return this.TargetUrl;
}

JavaScriptControls.GridRowHighLightExtender.prototype.set_TargetUrl = function(value)
{
    if (this.TargetUrl != value)
    {
        this.TargetUrl = value;
        this.raisePropertyChanged("TargetUrl");
    }
}

JavaScriptControls.GridRowHighLightExtender.prototype.get_IdList = function()
{
    return this.IdList;
}

JavaScriptControls.GridRowHighLightExtender.prototype.set_IdList = function(value)
{
    if (this.IdList != value)
    {
        this.IdList = value;
        this.raisePropertyChanged("IdList");
    }
}

// JavaScriptControls.GridRowHighLightExtender.registerClass('JavaScriptControls.GridRowHighLightExtender', Sys.UI.Behaviour);
JavaScriptControls.GridRowHighLightExtender.registerClass('JavaScriptControls.GridRowHighLightExtender', Sys.UI.Control);

Sys.Application.notifyScriptLoaded();
