import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-recipe',
    imports: [],
    templateUrl: './recipe-component.html',
    styleUrl: './recipe-component.scss'
})
export class RecipeComponent {
    @Input() title: string = "";

}
