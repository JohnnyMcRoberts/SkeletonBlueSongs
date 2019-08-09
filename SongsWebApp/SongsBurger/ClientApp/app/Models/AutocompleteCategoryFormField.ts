import { EventEmitter } from '@angular/core';
import { Validators, FormControl } from '@angular/forms';

import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';

export interface ICategoricalValuePair
{
    categoricalValue: string;
    count: number;
    keys: string[];
};

export class CategoricalValuePair implements ICategoricalValuePair
{
    static fromData(data: ICategoricalValuePair)
    {
        return new this(
            data.categoricalValue,
            data.count,
            data.keys);
    }

    constructor(
        public categoricalValue: string = null,
        public count: number = 0,
        public keys: string[] = new Array<string>()
    )
    { }
}

export interface IAutocompleteCategoryFormField
{
    optionsStrings: string[];
    optionValue: string;
    filteredValues: Observable<string[]>;

    form: FormControl;
};

export class AutocompleteCategoryFormField implements IAutocompleteCategoryFormField
{
    public optionsStrings: string[] = ['A. N. Other'];
    public optionValue: string = "";
    public filteredValues: Observable<string[]>;
    public form: FormControl;
    public change: EventEmitter<{}>;

    public filter(value: string): string[]
    {
        const filterValue = value.toLowerCase();

        var items = this.optionsStrings.filter(option => option.toLowerCase().indexOf(filterValue) === 0);

        return items;
    }

    public valueChanged(event: any)
    {
        this.optionValue = this.form.value;
        this.change.emit(event);
    }

    public setupSelectionOptions(newValues:string[]): void
    {
        // get a sorted version of the names
        const sortedValues = newValues.sort((t1, t2) =>
        {
            const name1 = t1.toLowerCase();
            const name2 = t2.toLowerCase();

            if (name1 > name2) { return 1; }
            if (name1 < name2) { return -1; }
            return 0;
        });

        // use the sorted version of the names
        this.optionsStrings = sortedValues;
        this.filteredValues = this.form.valueChanges.pipe(
            startWith(''),
            map(value => this.filter(value))
        );
    }

    public setupCategoricalOptions(categoricalValuePairs: ICategoricalValuePair[]): void
    {
        var newValues: string[] = new Array<string>();

        for (let valuePair of categoricalValuePairs)
        {
            if (valuePair.categoricalValue !== '')
               newValues.push(valuePair.categoricalValue);
        }

        this.setupSelectionOptions(newValues);
    }

    constructor(change: EventEmitter<{}>, required: boolean = false)
    {
        if (required)
            this.form = new FormControl('', Validators.required);
        else
            this.form = new FormControl('');

        this.change = change;
    }  
}
