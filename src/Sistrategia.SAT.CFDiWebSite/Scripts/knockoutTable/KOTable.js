//http://stackoverflow.com/questions/23087721/call-function-on-enter-key-press-knockout-js
ko.bindingHandlers.executeOnEnter = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var allBindings = allBindingsAccessor();
        $(element).keypress(function (event) {
            var keyCode = (event.which ? event.which : event.keyCode);
            if (keyCode === 13) {
                allBindings.executeOnEnter.call(viewModel);
                return false;
            }
            return true;
        });
    }
};

//http://craigcav.wordpress.com/2012/05/16/simple-client-storage-for-view-models-with-amplifyjs-and-knockout/
(function (ko) {
    ko.extenders.sessionStore = function (target, key) {
        var keyword = key;

        var value = amplify.store.sessionStorage(keyword) || target();

        var result = ko.computed({
            read: target,
            write: function (newValue) {
                amplify.store.sessionStorage(keyword, newValue);
                target(newValue);
            }
        });

        result(value);

        return result;
    };
})(ko);

function head(title, serverData, sortable) {
    var self = this;
    self.Title = ko.observable(title);
    self.ServerData = ko.observable(serverData);
    self.Sortable = ko.observable(sortable);
}

function sortProperty(sortOrder, serverData, sortable, tableId) {
    var self = this;
    self.SortOrder = ko.observable(sortOrder).extend({ sessionStore: tableId + "-sortOrder" });
    self.ServerData = ko.observable(serverData).extend({ sessionStore: tableId + "-serverData" });
    self.Sortable = ko.observable(sortable).extend({ sessionStore: tableId + "-sortable" });
}

function KOTable(tableId, url, headers, params, storage) {
    var self = this;
    var options = storage || true;

    //Begin Members
    self.Url = url;
    self.AjaxSuccess = ko.observable(true);
    self.DataList = ko.observableArray([]);
    self.Headers = ko.observableArray([]);

    self.Asc = ko.observable('ASC');
    self.Desc = ko.observable('DESC');
    self.CurrentSort = ko.observableArray([
        new sortProperty(null, null, null, tableId)
    ]);

    self.AvailableLength = ko.observableArray([10, 25, 50, 100]);
    self.SelectedLength = ko.observable(10).extend({ sessionStore: tableId + "-selectedLength" });
    self.PageSize = ko.observable(self.SelectedLength()).extend({ sessionStore: tableId + "-pageSize" });
    self.AjaxSuccess = ko.observable(true);

    self.SearchText = ko.observable().extend({ sessionStore: tableId + "-searchText" });
    self.HasSearchData = ko.observable(false);

    self.HasContacts = ko.observable(false);
    self.ToIndex = ko.observable(self.SelectedLength()).extend({ sessionStore: tableId + "-toIndex" });
    self.FromIndex = ko.observable(1).extend({ sessionStore: tableId + "-fromIndex" });
    self.Totalfields = ko.observable(0);

    self.Returnedfields = ko.observable(0);

    self.Page = ko.observable(1).extend({ sessionStore: tableId + "-page" });
    self.PageSize = ko.observable(self.SelectedLength()).extend({ sessionStore: tableId + "-pageSize" });
    self.PresentPage = ko.observable(1).extend({ sessionStore: tableId + "-presentPage" });
    self.CurrentPage = ko.observable(0).extend({ sessionStore: tableId + "-currentPage" });
    self.IsFirstPage = ko.observable('disabled').extend({ sessionStore: tableId + "-isFirstPage" });
    self.IsLastPage = ko.observable().extend({ sessionStore: tableId + "-isLastPage" });

    self.PresentPage = ko.observable(1).extend({ sessionStore: tableId + "-presentPage" });
    self.TotalPage = ko.observable(0);

    //End Members

    for (var i = 0; i < headers.length; i++) {
        self.Headers.push(new head(headers[i].cTitle, headers[i].sData, headers[i].sSortable));
    }

    //Register Length
    self.SelectedLength.subscribe(function (newValue) {
        self.PageSize(newValue);
        self.FromIndex(1);
        self.ToIndex(self.SelectedLength());

        self.PresentPage(1);
        self.CurrentPage(0);
        self.Page(1);

        self.ajaxRequest(self.Page(), self.PageSize(), self.CurrentSort()[0].ServerData(), self.CurrentSort()[0].SortOrder(), self.SearchText(), params);
    });

    //sorting
    self.SortBy = function (header, event, indice) {
        if (header.Sortable() == true) {
            if (header.ServerData() != null && self.HasContacts() == true) {
                if (self.CurrentSort()[0].ServerData() == null) {
                    self.CurrentSort()[0].ServerData(header.ServerData());
                    self.CurrentSort()[0].SortOrder(self.Asc());
                }
                else {
                    if (self.CurrentSort()[0].ServerData() != header.ServerData())
                        self.CurrentSort()[0].SortOrder(self.Asc());
                    else
                        self.CurrentSort()[0].SortOrder(self.CurrentSort()[0].SortOrder());

                    if (self.CurrentSort()[0].ServerData() == header.ServerData()) {
                        if (self.CurrentSort()[0].SortOrder() == 'ASC')
                            self.CurrentSort()[0].SortOrder(self.Desc());
                        else if (self.CurrentSort()[0].SortOrder() == 'DESC')
                            self.CurrentSort()[0].SortOrder(self.Asc());
                    }

                    self.CurrentSort()[0].ServerData(header.ServerData());

                }
                self.FromIndex(1);
                self.ToIndex(self.SelectedLength());
                self.PresentPage(1);
                self.CurrentPage(0);
                self.Page(1);
                self.ajaxRequest(self.Page(), self.PageSize(), self.CurrentSort()[0].ServerData(), self.CurrentSort()[0].SortOrder(), self.SearchText(), params);
            }
        }
    };

    //Searching
    self.EnterSearching = function () {
        self.Searching();
    };

    self.Searching = function () {
        if (self.SearchText()) {
            self.FromIndex(1);
            self.ToIndex(self.SelectedLength());
            self.PresentPage(1);
            self.CurrentPage(0);
            self.Page(1);
            self.IsFirstPage('disabled');
            self.IsLastPage('');
            self.ajaxRequest(self.Page(), self.PageSize(), self.CurrentSort()[0].ServerData(), self.CurrentSort()[0].SortOrder(), self.SearchText(), params);
        }
    };

    self.SearchText.subscribe(function (newValue) {
        self.HasSearchData(newValue && newValue.length ? true : false);
    });

    self.CleanSearch = function () {
        self.SearchText(null);
        self.CurrentSort()[0].ServerData(null);
        self.CurrentSort()[0].SortOrder(null);
        self.FromIndex(1);
        self.ToIndex(self.SelectedLength());
        self.PresentPage(1);
        self.CurrentPage(0);
        self.Page(1);
        self.ajaxRequest(self.Page(), self.PageSize(), self.CurrentSort()[0].ServerData(), self.CurrentSort()[0].SortOrder(), self.SearchText(), params);
    };

    //Details data
    self.DataList.subscribe(function (newValue) {
        self.HasContacts(newValue && newValue.length ? true : false);
    });

    self.Totalfields.subscribe(function (newValue) {
        if (self.SelectedLength() < newValue)
            self.ToIndex(self.SelectedLength());
        else
            self.ToIndex(newValue);
    });

    //Pagination
    self.FromIndex.subscribe(function (newValue) {
        if (newValue > 1)
            self.IsFirstPage('');
        else
            self.IsFirstPage('disabled');
    });

    self.PreviousPage = function () {
        if (self.FromIndex() > 1) {
            var page = self.CurrentPage() - self.SelectedLength();

            if (page <= 0) {
                self.CurrentPage(0);
                page = 1;
            }
            else {
                self.CurrentPage(page);
                page = page + 1;
            }
            self.FromIndex(page);

            var page_Size = (page - 1) + self.SelectedLength();
            self.ToIndex(page_Size);

            self.Page(self.Page() - 1);
            self.PageSize(self.SelectedLength());
            self.PresentPage(self.Page());

            self.ajaxRequest(self.Page(), self.PageSize(), self.CurrentSort()[0].ServerData(), self.CurrentSort()[0].SortOrder(), self.SearchText(), params);
        }
    };
    self.NextPage = function () {
        if (self.ToIndex() < self.Totalfields() && self.Returnedfields() <= self.ToIndex()) {
            (self.CurrentPage() > 0) ? self.CurrentPage(self.CurrentPage() + self.SelectedLength()) : self.CurrentPage(self.SelectedLength(), params);

            self.FromIndex(self.CurrentPage() + 1);

            //var page_Size = self.FromIndex() + self.Returnedfields() - 1;
            //if (page_Size > self.Totalfields()) {
            //    self.ToIndex(self.Totalfields());
            //}
            //else {
            //    self.ToIndex(page_Size);
            //}

            self.Page(self.Page() + 1);
            self.PageSize(self.SelectedLength());
            self.PresentPage(self.Page());

            self.ajaxRequest(self.Page(), self.PageSize(), self.CurrentSort()[0].ServerData(), self.CurrentSort()[0].SortOrder(), self.SearchText(), params);
        }
    };

    //Server 
    self.ajaxRequest = function (page, pageSize, sort, sortDir, searchText, params) {
       
        var object_data = {
            page : page || 1,
            pageSize : pageSize || 10,
            search : searchText || '',
            sort : sort || '',
            sortDir : sortDir || ''
        };
       
        for (var index in params) {
            if (params.hasOwnProperty(index)) {
                object_data[index] = params[index];
            }
        }

        $.ajax({
            url: self.Url,
            data: object_data,
            type: "POST",
            dataType: 'json',
            beforeSend: function () {
                self.AjaxSuccess(false);
            },
            success: function (data) {
                var mappedContacts = jQuery.makeArray(data);
                self.DataList(mappedContacts);
                self.AjaxSuccess(true);
                self.Totalfields(jQuery.isEmptyObject(data) ? 0 : parseInt(data[0]["total_rows"]));
                self.Returnedfields(parseInt(jQuery.isEmptyObject(data) ? 0 : data[0]["returned_rows"]));

                var page_Size = self.FromIndex() + self.Returnedfields() - 1;
                if (page_Size > self.Totalfields()) {
                    self.ToIndex(self.Totalfields());
                }
                else {
                    self.ToIndex(page_Size);
                }

                if (self.ToIndex() > self.Returnedfields() && self.ToIndex() >= self.Totalfields()) {
                    self.IsLastPage('disabled');
                    self.IsFirstPage('');
                }
                else if (self.ToIndex() <= self.Returnedfields() && self.ToIndex() < self.Totalfields()) {
                    self.IsLastPage('');
                    self.IsFirstPage('disabled');
                }
                else if (self.ToIndex() == self.Returnedfields() && self.ToIndex() == self.Totalfields()) {
                    self.IsLastPage('disabled');
                    self.IsFirstPage('disabled');
                }else{
                    self.IsLastPage('');
                    self.IsFirstPage('');
                }
                

                var totalPages = null;
                var totalPages = (self.Totalfields() / self.SelectedLength());
                   
                self.TotalPage(Math.ceil(totalPages, 1));

            },
            error: function (xhr, ajaxOptions, thrownError) {
                if (ajaxOptions == 'parsererror') {
                    self.CleanSearch();
                    //window.location.reload();
                }
                else {
                    console.log('Hubo un problema con la solicitud. ' + ajaxOptions + ' ' + xhr.status + ' - ' + thrownError);
                    //self.CleanSearch();
                }
            }
        });
    };

    self.ajaxRequest(self.Page(), self.PageSize(), self.CurrentSort()[0].ServerData(), self.CurrentSort()[0].SortOrder(), self.SearchText(), params);
}