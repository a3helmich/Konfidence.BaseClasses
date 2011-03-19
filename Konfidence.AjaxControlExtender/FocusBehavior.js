
Type.registerNamespace('AjaxExtender');

// constructor
AjaxExtender.FocusBehavior = function (element) 
{
    AjaxExtender.FocusBehavior.initializeBase(this, [element]);

    this.HighlightCssClass = null;
    this.NormalCssClass = null;
}

AjaxExtender.FocusBehavior.prototype.initialize = function() 
{
    AjaxExtender.FocusBehavior.callBaseMethod(this, 'initialize');

    $addHandlers(this.get_element(), { 'mouseover': this.onFocus, 'mouseout': this.onBlur }, this);

    this.get_element().className = this.NormalCssClass;
}

AjaxExtender.FocusBehavior.prototype.dispose = function() 
{
    $clearHandlers(this.get_element());

    AjaxExtender.FocusBehavior.callBaseMethod(this, 'dispose');
}

AjaxExtender.FocusBehavior.prototype.onFocus = function()
{
    if (this.get_element() && !this.get_element().disabled) 
    {
        this.get_element().className = this.HighlightCssClass;
    }
}

AjaxExtender.FocusBehavior.prototype.onBlur = function() 
{
    if (this.get_element() && !this.get_element().disabled)
    {
        this.get_element().className = this.NormalCssClass;
    }
}

AjaxExtender.FocusBehavior.prototype.get_HighLightCssClass = function() 
{
    return this.HighlightCssClass;
}

AjaxExtender.FocusBehavior.prototype.set_HighLightCssClass = function(value) 
{
    if (this.HighlightCssClass != value) 
    {
        this.HighlightCssClass = value;
        this.raisePropertyChanged('HighlightCssClass');
    }
}

AjaxExtender.FocusBehavior.prototype.get_NormalCssClass = function ()
 {
    return this.NormalCssClass;
}

AjaxExtender.FocusBehavior.prototype.set_NormalCssClass = function (value) 
{
    if (this.NormalCssClass != value) 
    {
        this.NormalCssClass = value;
        this.raisePropertyChanged('NormalCssClass');
    }
}

AjaxExtender.FocusBehavior.registerClass('AjaxExtender.FocusBehavior', Sys.UI.Behavior);

Sys.Application.notifyScriptLoaded();
